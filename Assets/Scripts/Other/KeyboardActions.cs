using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class KeyboardActions : MonoBehaviour {

    public GuiManager guiManager;

    private AudioManager audioManager;    

    private InputActionAsset actionAsset;

    private InputActionMap keyPlay;
    private InputActionMap keyMod;

    private Dictionary<string, Dictionary< string, Action<InputAction.CallbackContext> >> storedActions;

    #region Unity
    //public void RegenerateActions() {
    //    storedActions.Clear();
    //    SetupActionAssets();
    //    CreateActions();
    //}
    void Awake() {
        storedActions = new Dictionary<string, Dictionary<string, Action<InputAction.CallbackContext>>>();
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

    private void Start() {
        audioManager = GetComponent<AudioManager>();
    }

    #endregion

    #region Keyboard Setup
    void SetupActionAssets() {
        actionAsset = ScriptableObject.CreateInstance<InputActionAsset>();

        actionAsset.AddActionMap(keyPlay = new InputActionMap("keyPlay"));
        actionAsset.AddActionMap(keyMod = new InputActionMap("keyMod"));
        
        storedActions.Add("keyPlay", new Dictionary<string, Action<InputAction.CallbackContext>>());
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
                ctx => {
                    if (guiManager.OctaveCount >= 3 || key.Value < 12) {
                        audioManager.OnNote(ctx, key.Value, key.Key);
                    }
                },
                "<Keyboard>/" + key.Key,
                "press(behavior=0)"
            );

            StoreAction(ref keyPlay,
                "offNote_" + key.Key,
                ctx => {
                    if (guiManager.OctaveCount >= 3 || key.Value < 12) {
                        audioManager.OffNote(ctx, key.Value, key.Key);
                    }
                },
                "<Keyboard>/" + key.Key,
                "press(behavior=1)"
            );
        }

        StoreAction(ref keyMod, "upOctave", _ => audioManager.ChangeOctave(1), "<Keyboard>/equals");
        StoreAction(ref keyMod, "downOctave", _ => audioManager.ChangeOctave(-1), "<Keyboard>/minus");
    }

    void SubscribeActions() {
        foreach (var map in storedActions) {
            foreach (var action in actionAsset.FindActionMap(map.Key).actions) {
                action.performed += context => map.Value[action.name](context);
            }
        }
    }

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

    #endregion
}