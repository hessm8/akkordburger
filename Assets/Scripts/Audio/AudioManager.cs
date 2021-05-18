using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using MidiToolkit;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {   

    private double BeatInMilliseconds => 60000 / BPM * 0.001;
    [Range(100, 200)]
    public int BPM = 120;
    [Range(2, 8)]
    public int Octave = 4;

    Dropdown instrumentUI;
    Dictionary<string, Slider> mainSliders = new Dictionary<string, Slider>();

    public bool allowPlay = true;

    readonly List<Instrument> instruments = new List<Instrument>() {
        new Instrument("Piano", 0, 0),
        new Instrument("Harpsichord", 0, 6),
        new Instrument("Glockenspiel", 1, 9),
        new Instrument("Accordion", 2, 21),
        new Instrument("Acoustic Guitar", 3, 24),
        new Instrument("Rock Guitar", 3, 29)
    };

    public Instrument currentInstrument = new Instrument();

    private void ChangeDropdown(int index) {
        currentInstrument = instruments[index];
    }

    private void AddMainControls() {
        Utils.ForeachChildOf("Settings", child => {
            var name = child.name;

            var label = child.transform.GetChild(0);
            var textComponent = label.GetComponent<Text>();
            textComponent.text = name;

            if (name == "Instrument") {
                instrumentUI = child.GetComponent<Dropdown>();             
            } else {
                mainSliders.Add(name, child.GetComponent<Slider>());
            }
        });
    }

    private void AddControls() {
        new ReverbControl(this);
        new DelayControl(this);
        new ChorusControl(this);

        AddMainControls();

        instrumentUI.ClearOptions();
        instrumentUI.AddOptions(instruments.Select(i => i.Name).ToList());

        instrumentUI.onValueChanged.AddListener(ChangeDropdown);
    }

    public void Awake() {
        Akkordburger.AudioManager = this;
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

    private List<PianoKey> SustainedNotes = new List<PianoKey>();

    public void ChangeOctave(int direction) {      

        while (SustainedNotes.Count > 0) {
            var note = SustainedNotes[0];
            note.OnPointerUp(PointerData);

            var button = note.GetComponent<Button>();
            button.OnPointerUp(PointerData);
        }

        Octave += direction;
    }

    public void OnPianoKey(PianoKey key, bool state) {

        var playState = state ? MidiCommand.NoteOn : MidiCommand.NoteOff;

        NotePlaying = new MidiEvent() {
            Command = playState,
            Value = Note(key.Index),
            Channel = currentInstrument.Channel, // from 0 to 15, 9 reserved for drum
            Duration = -1, // note duration in millisecond, -1 to play undefinitely, MPTK_StopChord to stop
            Velocity = 75, // from 0 to 127, sound can vary depending on the velocity
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
