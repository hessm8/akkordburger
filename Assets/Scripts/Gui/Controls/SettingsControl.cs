using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsControl : UIControl {
    public SettingsControl(AudioManager manager) : base(manager) { }
    public override string ControlGroup => "Settings";

    private Dropdown instrument;

    protected override void Locate(GameObject child) {

        if (child.HasComponent<Dropdown>(out var dropdown)) {
            instrument = dropdown;
        }

    }
    private void ChangeDropdown(int index) {
        Manager.currentInstrument = Instrument.Presets[index];
    }
    protected override void Initialize() {
        instrument.ClearOptions();
        instrument.AddOptions(Instrument.Presets.Select(i => i.Name).ToList());        
    }
    protected override void AddEvents() {
        instrument.onValueChanged.AddListener(ChangeDropdown);
    }
}