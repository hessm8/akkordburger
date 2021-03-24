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

    private InputActionAsset actionAsset;

    private InputActionMap keyPlay;
    private InputActionMap keyMod;

    private Dictionary <
        string, 
        Dictionary <
            string,
            Action<InputAction.CallbackContext>
            >
        > storedActions;

    #region Unity
    void Awake() {
        SetupActionAssets();
        CreateActions();
    }
    private void OnEnable() {
        actionAsset.Enable();
        SubscribeActions();
    }
    private void OnDisable() {
        actionAsset.Disable();
    }

    #endregion

    #region Keyboard Setup
    void SetupActionAssets() {
        actionAsset = ScriptableObject.CreateInstance<InputActionAsset>();

        actionAsset.AddActionMap(keyPlay = new InputActionMap("keyPlay"));
        actionAsset.AddActionMap(keyMod = new InputActionMap("keyMod"));

        storedActions = new Dictionary<string, Dictionary<string, Action<InputAction.CallbackContext>>>();
        storedActions.Add("keyPlay", new Dictionary <string, Action<InputAction.CallbackContext>>());
        storedActions.Add("keyMod", new Dictionary<string, Action<InputAction.CallbackContext>>());
    }

    void StoreAction(ref InputActionMap map, string name, Action<InputAction.CallbackContext> action,
        string bind = null, string interactions = null) {
        map.AddAction(name, binding: bind, interactions: interactions);
        storedActions[map.name].Add(name, action);
    }

    void CreateActions() {
        //var actionMap = actionAsset.FindActionMap("keyPlay");
        foreach (var key in keys) {
            StoreAction(ref keyPlay,
                "onNote_" + key.Key,
                _ => {
                    NotePlaying = new MPTKEvent() {
                        Command = MPTKCommand.NoteOn,
                        Value = 48,
                        Channel = 0,
                        Duration = -1,
                        Velocity = 100,
                        Delay = 0,
                    };
                    midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

                    Debug.Log("onNote_" + key.Key + " played");
                },
                "<Keyboard>/" + key.Key,
                "press(behavior=0)"
            );

            StoreAction(ref keyPlay,
                "offNote_" + key.Key,
                _ => NoteControl(key.Value, false),
                "<Keyboard>/" + key.Key,
                "press(behavior=1)"
            );
        }

        StoreAction(ref keyMod, "upOctave", _ => octave++, "<Keyboard>/equals");
        StoreAction(ref keyMod, "downOctave", _ => octave--, "<Keyboard>/minus");
    }

    void SubscribeActions() {
        foreach (var map in storedActions) {            
            foreach (var action in actionAsset.FindActionMap(map.Key).actions) {
                action.performed += context => map.Value[action.name](context);
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