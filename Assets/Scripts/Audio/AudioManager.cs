using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using MidiToolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {

    public GuiManager guiManager;

    public float BeatInMilliseconds => 60000 / BPM;
    [Range(100, 200)]
    public int BPM = 120;
    [Range(2, 8)]
    public int Octave = 5;

    public bool allowPlay = true;    
    public Instrument currentInstrument = new Instrument();
    private SettingsControl settings;
    public DelayControl delay;
    private ReverbControl reverb;
    private ChorusControl chorus;
    private void AddControls() {
        reverb = new ReverbControl(this);
        delay = new DelayControl(this);
        chorus = new ChorusControl(this);
        settings = new SettingsControl(this);
    }

    public void Awake() {
        General.AudioManager = this;
        Player = GetComponent<MidiStreamPlayer>();
    }

    public void Start() {           
        AddControls();
    }

    #region Note Events 

    public MidiStreamPlayer Player { get; private set; }
    private MidiEvent NotePlaying;
    private EventSystem events => EventSystem.current;
    private PointerEventData PointerData => new PointerEventData(events);

    #region Keyboard
    private void GetKey(int keyIndex, out PianoKey pianoKey, out Button guiKey) {
        var gameObject = GameObject.Find($"PianoKey_{keyIndex}");
        pianoKey = gameObject.GetComponent<PianoKey>();
        guiKey = gameObject.GetComponent<Button>();
    }
    internal void OnNote(InputAction.CallbackContext context, int key, string computerKey = null) {
        if (!allowPlay) return;
        GetKey(key, out var keyPiano, out var keyGui);

        keyGui.OnPointerDown(PointerData);
        keyPiano.OnPointerDown(PointerData);
    }
    internal void OffNote(InputAction.CallbackContext context, int key, string computerKey = null) {
        if (!allowPlay) return;
        GetKey(key, out var keyPiano, out var keyGui);

        keyGui.OnPointerUp(PointerData);
        keyPiano.OnPointerUp(PointerData);
    }
    #endregion

    public readonly List<PianoKey> SustainedNotes = new List<PianoKey>();

    public void ChangeOctave(int direction, bool fromGUI = false) {
        int newOctave = Octave + direction;
        if (!newOctave.In(1, 9)) return;

        while (SustainedNotes.Count > 0) {
            var note = SustainedNotes[0];
            note.OnPointerUp(PointerData);

            var button = note.GetComponent<Button>();
            button.OnPointerUp(PointerData);
        }

        if (settings != null && !fromGUI) settings.Sliders["Octave"].value = newOctave;
        Octave = newOctave;
    }


    public int NoteVolume = 100;

    public void OnPianoKey(PianoKey key, bool state) {

        var playState = state ? MidiCommand.NoteOn : MidiCommand.NoteOff;

        NotePlaying = new MidiEvent() {
            Command = playState,
            Value = Note(key.Index),
            Channel = currentInstrument.Channel, // from 0 to 15, 9 reserved for drum
            Duration = -1, // note duration in millisecond, -1 to play undefinitely, MPTK_StopChord to stop
            Velocity = NoteVolume, // from 0 to 127, sound can vary depending on the velocity
            Delay = 0, // delay in millisecond before playing the note
        };

        if (state) SustainedNotes.Add(key);
        else SustainedNotes.Remove(key);
        currentInstrument.Use();

        Player.PlayAudioEvent(NotePlaying);
    }

    private int Note(int keyIndex) => 12 * Octave + keyIndex;

    //    timer += Time.fixedDeltaTime;

    //if (timer > BeatInMilliseconds + offset) {
    //if (noteind != melody.Length-1) SendNoteEvent(12 * 5 + melody[noteind++]);
    //    noteind %= melody.Length;
    //timer -= (float) BeatInMilliseconds;
    //}

    #endregion
}
