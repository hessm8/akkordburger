using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {    
    public int Index { get; set; }

    private AudioManager audioManager;

    void Start() {
        audioManager = GameObject.Find("MidiPlayer").GetComponent<AudioManager>();
    }

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData) {
        audioManager.OnPianoKey(this, true);
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData) {
        audioManager.OnPianoKey(this, false);
    }    
}
