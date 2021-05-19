using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
    public Button keyPrefab;
    public KeyboardActions actions;

    readonly Vector2 keySize = new Vector2(30, 150);
    const int noteCount = 12;
    const int whiteCount = 7;
    public int OctaveCount { get; set; } = 3;
    float KeyOffset => (keySize.x * whiteCount * OctaveCount) / 2;

    readonly HashSet<int> BlackKeys = new HashSet<int> { 1, 3, 6, 8, 10 };
    readonly List<Button> AllKeys = new List<Button>();
    public void CreateKeyboardUI() {
        DrawOctaves(OctaveCount);
    }
    public void RedoKeyboardUI(int addOctave) {
        var changed = OctaveCount + addOctave;
        if (!changed.In(1, 6)) return;

        foreach (var k in AllKeys) {
            Destroy(k.gameObject);
        }

        OctaveCount = changed;

        AllKeys.Clear();
        drawBlack.Clear();
        General.AudioManager.SustainedNotes.Clear();

        CreateKeyboardUI();
    }

    List<UnityAction> drawBlack = new List<UnityAction>();
    private void DrawOctaves(int octaves) {
        int addOffsetCount = 0;

        for (int keyIndex = 0; keyIndex < noteCount * octaves; keyIndex++) {
            DrawKeyAt(keyIndex, ref addOffsetCount);
        }

        foreach (var drawKey in drawBlack) drawKey();
    }
    void DrawKeyAt(int k, ref int addOffsetCount) {
        var inOctave = k % noteCount;
        bool isBlack = BlackKeys.Contains(inOctave);

        float addOffset = keySize.x / 2;
        if (inOctave == 5 || inOctave == 0) {
            addOffsetCount++;
        }

        var keyPos = new Vector3(k * keySize.x / 2 - KeyOffset + addOffset * addOffsetCount, 0);

        UnityAction drawKey = () => {
            var keyObject = Instantiate(keyPrefab, keyPos, Quaternion.identity);
            keyObject.name = $"PianoKey_{k - 12}";

            SetSpecific(ref keyObject, isBlack);

            keyObject.transform.SetParent(transform, false);
            keyObject.GetComponent<PianoKey>().Index = k;

            AllKeys.Add(keyObject);
        };       

        if (isBlack) drawBlack.Add(drawKey);
        else drawKey();
    }
    void SetSpecific(ref Button keyObject, bool isBlack) {
        var keyMainColor = Color.white;

        if (isBlack) {
            var rt = keyObject.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 22.5f);

            rt.position += new Vector3(0, 30);
            keyMainColor = Color.black;
        }

        var colors = new ColorBlock() {
            pressedColor = Color.Lerp(keyMainColor, General.KeyColor, 0.45f),
            normalColor = keyMainColor,
            highlightedColor = keyMainColor,
            selectedColor = keyMainColor,
            colorMultiplier = 1,
            fadeDuration = 0.06f,            
        };
        keyObject.colors = colors;
    }

    void Start() {
        CreateKeyboardUI();
    }
}
