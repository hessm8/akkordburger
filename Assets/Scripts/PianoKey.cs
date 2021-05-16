using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private KeyboardActions midiSuite;
    public int keyIndex;

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData) {
        midiSuite.NoteControl(keyIndex, true);
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData) {
        midiSuite.NoteControl(keyIndex, false);
    }

    void Start() {
        midiSuite = GameObject.Find("MidiStreamPlayer").GetComponent<KeyboardActions>();
    }
}
