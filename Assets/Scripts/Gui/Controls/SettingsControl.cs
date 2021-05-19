using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : UIControl {
    public SettingsControl(AudioManager manager) : base(manager) { }
    public override string ControlGroup => "Settings";

    private Dropdown instrument;
    private FieldBPM bpm;

    protected override void Locate(GameObject child) {

        if (child.HasComponent<Dropdown>(out var dropdown)) {
            instrument = dropdown;
        } else if (child.HasComponent<FieldBPM>(out var field)) {
            bpm = field;
        }

    }
    private void ChangeDropdown(int index) {
        Manager.currentInstrument = Instrument.Presets[index];
    }
    protected override void Initialize() {
        instrument.ClearOptions();
        instrument.AddOptions(Instrument.Presets.Select(i => i.Name).ToList());

        bpm.audioManager = Manager;
        bpm.text = Manager.BPM.ToString();
    }
    protected override void AddEvents() {
        instrument.onValueChanged.AddListener(ChangeDropdown);
        Slider("Volume", (0, 100), value => Manager.NoteVolume = (int)value);
        Slider("Octave", (2, 8), value => {
            var diff = (int)value - Manager.Octave;
            if (diff != 0) Manager.ChangeOctave(diff, true);
        });
    }
}