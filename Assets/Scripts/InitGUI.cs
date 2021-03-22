using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitGUI : MonoBehaviour {
    public Button key;
    public KeyboardTest midiSuite;

    Vector2 keySize = new Vector2(30, 150);
    int quantity = 12;
    int octaves = 3;
    float keyOffset => (keySize.x * quantity * octaves) / (2 * 1.7f);

    List<int> blackers = new List<int> { 1, 3, 6, 8, 10 };

    private int keyIndexOctave(int keyIndex) => keyIndex % quantity;
    void CreateKeyboardUI() {
        int addOffsetCount = 0;

        var toDo = new List<Action>();

        for (int keyIndex = 0; keyIndex < quantity * octaves; keyIndex++) {
            bool isBlack = blackers.Contains(keyIndexOctave(keyIndex));

            float addOffset = keySize.x / 2;
            if (keyIndexOctave(keyIndex) == 5 || keyIndexOctave(keyIndex) == 0) {
                addOffsetCount++;
            }

            Button keyObject;
            var keyPos = new Vector3(keyIndex * keySize.x / 2 - keyOffset + addOffset * addOffsetCount, 0);

            Action drawKey = () => {
                keyObject = Instantiate(key, keyPos, Quaternion.identity);

                if (isBlack) {
                    var rt = keyObject.GetComponent<RectTransform>();
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25);

                    rt.position += new Vector3(0, 30);

                    var col = keyObject.colors;
                    col.normalColor = Color.black;
                    keyObject.colors = col;
                }

                keyObject.transform.SetParent(transform, false);
                keyObject.GetComponent<PianoKey>().keyIndex = keyIndex;
            };

            if (isBlack) toDo.Add(drawKey);
            else drawKey();
        }

        toDo.ForEach(a => a.Invoke());
    }

    void CreateKeyboardUIOld() {
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
        CreateKeyboardUI();
    }
}
