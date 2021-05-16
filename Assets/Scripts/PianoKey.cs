using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private AudioManager audioManager;
    public int keyIndex;

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData) {
        audioManager.NoteControl(keyIndex, true);
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData) {
        audioManager.NoteControl(keyIndex, false);
    }

    void Start() {
        audioManager = GameObject.Find("MidiPlayer").GetComponent<AudioManager>();
    }
}
