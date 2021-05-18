using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {    
    public int Index { get; set; }

    private AudioManager audioManager = Akkordburger.AudioManager;

    public void OnPointerDown(PointerEventData eventData) {
        audioManager.OnPianoKey(this, true);
    }

    public void OnPointerUp(PointerEventData eventData) {
        audioManager.OnPianoKey(this, false);
    }    
}
