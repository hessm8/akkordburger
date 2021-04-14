using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InitGUI : MonoBehaviour {
    public Button key;
    public KeyboardTest midiSuite;

    Vector2 keySize = new Vector2(30, 150);
    int quantity = 12;
    int octaves = 3;
    float keyOffset => (keySize.x * quantity * octaves) / (2 * 1.7f);

    List<int> blackers = new List<int> { 1, 3, 6, 8, 10 };

    private int keyIndexInOctave(int keyIndex) => keyIndex % quantity;

    void CreateKeyboardUI() {
        DrawOctaves(3);
    }

    List<UnityAction> drawLater;
    private void DrawOctaves(int octaves) {
        int addOffsetCount = 0;
        for (int keyIndex = 0; keyIndex < quantity * octaves; keyIndex++) {
            DrawKeyAt(keyIndex, ref addOffsetCount);
        }
        foreach (var drawKey in drawLater) drawKey();
    }

    void DrawKeyAt(int k, ref int addOffsetCount) {
        bool isBlack = blackers.Contains(keyIndexInOctave(k));

        float addOffset = keySize.x / 2;
        if (keyIndexInOctave(k) == 5 || keyIndexInOctave(k) == 0) {
            addOffsetCount++;
        }

        Button keyObject;
        var keyPos = new Vector3(k * keySize.x / 2 - keyOffset + addOffset * addOffsetCount, 0);

        UnityAction drawKey = () => {
            keyObject = Instantiate(key, keyPos, Quaternion.identity);
            keyObject.name = $"PianoKey_{k - 12}";

            SetSpecific(ref keyObject, isBlack);

            keyObject.transform.SetParent(transform, false);
            keyObject.GetComponent<PianoKey>().keyIndex = k;
        };

        //drawKey();

        if (isBlack) drawLater.Add(drawKey);
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

        var keyPressedColor = new Color(0.196f, 0.75f, 0.58f);
        var colors = new ColorBlock() {
            pressedColor = Color.Lerp(keyMainColor, keyPressedColor, 0.45f),
            normalColor = keyMainColor,
            highlightedColor = keyMainColor,
            selectedColor = keyMainColor,
            colorMultiplier = 1,
            fadeDuration = 0.06f,
            
        };
        keyObject.colors = colors;
    }


    void CreateKeyboardUIOld() {
        int addOffsetCount = 0;

        var toDo = new List<UnityAction>();

        for (int keyIndex = 0; keyIndex < quantity * octaves; keyIndex++) {
            bool isBlack = blackers.Contains(keyIndexInOctave(keyIndex));

            float addOffset = keySize.x / 2;
            if (keyIndexInOctave(keyIndex) == 5 || keyIndexInOctave(keyIndex) == 0) {
                addOffsetCount++;
            }

            Button keyObject;
            var keyPos = new Vector3(keyIndex * keySize.x / 2 - keyOffset + addOffset * addOffsetCount, 0);

            UnityAction drawKey = () => {
                keyObject = Instantiate(key, keyPos, Quaternion.identity);

                //if (isBlack) SetBlack(ref keyObject);

                keyObject.transform.SetParent(transform, false);
                keyObject.GetComponent<PianoKey>().keyIndex = keyIndex;
            };

            if (isBlack) toDo.Add(drawKey);
            else drawKey();
        }

        toDo.ForEach(a => a.Invoke());
    }

    void CreateKeyboardUIFirst() {
        for (int keyIndex = 0; keyIndex < quantity * octaves; keyIndex++) {
            var keyPos = new Vector3(keyIndex * keySize.x - keyOffset, 0);
            var keyObject = Instantiate(key, keyPos, Quaternion.identity);

            keyObject.transform.SetParent(transform, false);
            keyObject.GetComponent<PianoKey>().keyIndex = keyIndex;
        }

        for (int keyIndex = 0; keyIndex < quantity * octaves; keyIndex++) {
            var noteThing = keyIndex % 7;
            if (noteThing == 2 || noteThing == 6) continue;

            var keyPos = new Vector3(keyIndex * keySize.x - keyOffset + keySize.x / 2, 0);
            var keyObject = Instantiate(key, keyPos, Quaternion.identity);

            var rt = keyObject.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25);

            rt.position += new Vector3(0, 30);

            var col = keyObject.colors;
            col.normalColor = Color.black;
            keyObject.colors = col;

            keyObject.transform.SetParent(transform, false);
            keyObject.GetComponent<PianoKey>().keyIndex = keyIndex;
        }
    }

    void Start() {
        drawLater = new List<UnityAction>();
        CreateKeyboardUI();
    }
}
