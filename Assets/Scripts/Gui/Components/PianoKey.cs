using UnityEngine;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {    
    public int Index { get; set; }

    private AudioManager audioManager = General.AudioManager;

    public void OnPointerDown(PointerEventData eventData) {
        audioManager.OnPianoKey(this, true);
    }

    public void OnPointerUp(PointerEventData eventData) {
        audioManager.OnPianoKey(this, false);
    }    
}
