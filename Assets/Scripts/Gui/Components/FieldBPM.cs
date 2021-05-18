using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBPM : InputField {

    private AudioManager audioManager = Akkordburger.AudioManager;

    protected override void Start() {
        base.Start();
        audioManager = Akkordburger.AudioManager;
        text = audioManager.BPM.ToString();

        onEndEdit.AddListener(OnLeave);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);

        audioManager.allowPlay = false;
    }

    private void OnLeave(string input) {
        if (!int.TryParse(input, out var number) || !number.In(100, 200)) {
            text = audioManager.BPM.ToString();
        } else audioManager.BPM = number;

        audioManager.allowPlay = true;
    }
}
