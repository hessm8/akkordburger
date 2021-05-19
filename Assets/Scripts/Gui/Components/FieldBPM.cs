using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldBPM : InputField {

    internal AudioManager audioManager = Akkordburger.AudioManager;

    protected override void Start() {
        base.Start();

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

        audioManager.delay.ChangeDelayTime();
    }
}
