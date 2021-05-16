﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using MidiPlayerTK;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {   

    private double BeatInMilliseconds => 60000 / BPM * 0.001;
    [Range(100, 200)]
    public int BPM = 120;
    [Range(2, 8)]
    public int Octave = 4;

    public void AddEffects() {
        new ReverbControl(this);
        new DelayControl(this);
        new ChorusControl(this);
    }

    public void Start() {        
        AddEffects();
        midiPlayer = GetComponent<MidiStreamPlayer>();
    }

    #region Note Events 

    private MidiStreamPlayer midiPlayer;
    private MPTKEvent NotePlaying;
    private PointerEventData PointerData => new PointerEventData(EventSystem.current);

    #region Keyboard
    private void GetKey(int keyIndex, out PianoKey pianoKey, out Button guiKey) {
        var gameObject = GameObject.Find($"PianoKey_{keyIndex}");
        pianoKey = gameObject.GetComponent<PianoKey>();
        guiKey = gameObject.GetComponent<Button>();
    }
    internal void OnNote(InputAction.CallbackContext context, int key, string computerKey = null) {
        GetKey(key, out var keyPiano, out var keyGui);

        keyGui.OnPointerDown(PointerData);
        keyPiano.OnPointerDown(PointerData);
    }
    internal void OffNote(InputAction.CallbackContext context, int key, string computerKey = null) {
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

        var playState = state ? MPTKCommand.NoteOn : MPTKCommand.NoteOff;

        NotePlaying = new MPTKEvent() {
            Command = playState,
            Value = Note(key.Index),
            Channel = 0, // from 0 to 15, 9 reserved for drum
            Duration = -1, // note duration in millisecond, -1 to play undefinitely, MPTK_StopChord to stop
            Velocity = 75, // from 0 to 127, sound can vary depending on the velocity
            Delay = 0, // delay in millisecond before playing the note
        };

        if (state) SustainedNotes.Add(key);
        else SustainedNotes.Remove(key);

        midiPlayer.MPTK_PlayEvent(NotePlaying);
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
