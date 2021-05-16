using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioEffectManager : MonoBehaviour {

    private Dictionary<string, Behaviour> effects = new Dictionary<string, Behaviour>();

    public void Start() {
        effects.Add("reverb", GetComponent<AudioReverbFilter>());
        effects.Add("delay", GetComponent<AudioEchoFilter>());
        effects.Add("chorus", GetComponent<AudioChorusFilter>());
    }

    public void Toggle(string effectName) {
        var effect = effects[effectName];
        effect.enabled = !effect.enabled;
    }
}
