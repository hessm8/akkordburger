using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using MidiPlayerTK;
using System;

public class KeyboardTest : MonoBehaviour {
    public MidiStreamPlayer midiStreamPlayer;
    private MPTKEvent NotePlaying;

    public int BPM;

    private double BeatInMilliseconds => 60000 / BPM * 0.001;
    [Range(1, 9)]
    public int octave = 4;

    private List<InputAction> onNote;
    private InputAction offNote;

    private InputActionAsset actionAsset;
    private InputActionMap playKeyboard;

    #region Unity
    void Awake() {
        SetupActionAssets();
        CreateActions();
    }
    private void OnEnable() {
        playKeyboard.Enable();
        SubscribeActions();
    }
    private void OnDisable() {
        playKeyboard.Disable();
    }

    #endregion

    #region Keyboard Setup
    void SetupActionAssets() {
        actionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        playKeyboard = new InputActionMap("playKeyboard");
        actionAsset.AddActionMap(playKeyboard);
    }
    void CreateActions() {
        foreach (var key in keys) {
            playKeyboard.AddAction("onNote_" + key.Key)
                .AddBinding("<Keyboard>/" + key.Key)
                .WithInteraction("press(behavior=0)");
            playKeyboard.AddAction("offNote_" + key.Key)
                .AddBinding("<Keyboard>/" + key.Key)
                .WithInteraction("press(behavior=1)");
        }

        playKeyboard.AddAction("upOctave").AddBinding("<Keyboard>/equals");
        playKeyboard.AddAction("downOctave").AddBinding("<Keyboard>/minus");

    }
    void SubscribeActions() {
        foreach (var action in playKeyboard.actions) {
            var name = action.name;
            switch (name) {
                case "upOctave": {
                    action.performed += _ => octave++;
                    break;
                }
                case "downOctave": {
                    action.performed += _ => octave--;
                    break;
                }
                default: {
                    var keyLetter = name[name.Length - 1].ToString();
                    action.performed += _ => NoteControl(keys[keyLetter], action.name.Contains("onNote"));
                    break;
                }
            }
        }
    }

    #endregion

    #region Note Functions

    Dictionary<string, int> keys = new Dictionary<string, int>() {
        { "Q", 0 },
        { "2", 1 },
        { "W", 2 },
        { "3", 3 },
        { "E", 4 },
        { "R", 5 },
        { "5", 6 },
        { "T", 7 },
        { "6", 8 },
        { "Y", 9 },
        { "7", 10 },
        { "U", 11 },
        { "I", 12 },
        { "9", 13 },
        { "O", 14 },
        { "0", 15 },
        { "P", 16 },
        { "Z", -12 },
        { "S", -11 },
        { "X", -10 },
        { "D", -9 },
        { "C", -8 },
        { "V", -7 },
        { "G", -6 },
        { "B", -5 },
        { "H", -4 },
        { "N", -3 },
        { "J", -2 },
        { "M", -1 },
    };

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
        midiStreamPlayer.MPTK_PlayEvent(NotePlaying);
    }

    public void NoteControl(int keyIndex, bool state) {
        SendNoteEvent(12 * octave + keyIndex, state);
    }

    //    timer += Time.fixedDeltaTime;

    //if (timer > BeatInMilliseconds + offset) {
    //if (noteind != melody.Length-1) SendNoteEvent(12 * 5 + melody[noteind++]);
    //    noteind %= melody.Length;
    //timer -= (float) BeatInMilliseconds;
    //}

    #endregion
}