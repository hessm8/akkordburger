using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using MidiPlayerTK;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {
    [Range(100, 200)]
    public int BPM = 120;
    private double BeatInMilliseconds => 60000 / BPM * 0.001;
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

    internal void OnNote(InputAction.CallbackContext context, int key, string computerKey = null) {
        var keyObj = GameObject.Find($"PianoKey_{key}");
        var pianoKey = keyObj.GetComponent<PianoKey>();
        var guiKey = keyObj.GetComponent<Button>();
        var d = new PointerEventData(EventSystem.current);

        guiKey.OnPointerDown(d);
        pianoKey.OnPointerDown(d);

        //NotePlaying = new MPTKEvent() {
        //    Command = MPTKCommand.NoteOn,
        //    Value = 48 + key,
        //    Channel = 0,
        //    Duration = -1,
        //    Velocity = 100,
        //    Delay = 0,
        //};
        //midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

        Debug.Log("onNote_" + computerKey + " played");
    }

    internal void OffNote(InputAction.CallbackContext context, int key, string computerKey = null) {
        var keyObj = GameObject.Find($"PianoKey_{key}");
        var pianoKey = keyObj.GetComponent<PianoKey>();
        var guiKey = keyObj.GetComponent<Button>();
        var d = new PointerEventData(EventSystem.current);

        guiKey.OnPointerUp(d);
        pianoKey.OnPointerUp(d);

        //NotePlaying = new MPTKEvent() {
        //    Command = MPTKCommand.NoteOn,
        //    Value = 48 + key,
        //    Channel = 0,
        //    Duration = -1,
        //    Velocity = 100,
        //    Delay = 0,
        //};
        //midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

        Debug.Log("offNote_" + computerKey + " played");
    }    

    private void SendNoteEvent(int note = 48, bool isPlaying = true) {
        var playState = isPlaying ? MPTKCommand.NoteOn : MPTKCommand.NoteOff;

        NotePlaying = new MPTKEvent() {
            Command = playState, // midi command
            Value = note, // from 0 to 127, 48 for C4, 60 for C5, ...
            Channel = 0, // from 0 to 15, 9 reserved for drum
            Duration = -1, // note duration in millisecond, -1 to play undefinitely, MPTK_StopChord to stop
            Velocity = 100, // from 0 to 127, sound can vary depending on the velocity
            Delay = 0, // delay in millisecond before playing the note
        };
        midiPlayer.MPTK_PlayEvent(NotePlaying);
    }

    public void NoteControl(int keyIndex, bool state) {
        SendNoteEvent(12 * Octave + keyIndex, state);
    }

    //    timer += Time.fixedDeltaTime;

    //if (timer > BeatInMilliseconds + offset) {
    //if (noteind != melody.Length-1) SendNoteEvent(12 * 5 + melody[noteind++]);
    //    noteind %= melody.Length;
    //timer -= (float) BeatInMilliseconds;
    //}

    #endregion
}
