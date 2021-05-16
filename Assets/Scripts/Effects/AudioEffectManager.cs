﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioEffectManager : MonoBehaviour {

    //private Dictionary<string, Behaviour> effects = new Dictionary<string, Behaviour>();
    private Dictionary<string, IEffectControl> controls = new Dictionary<string, IEffectControl>();

    public void AddEffects(params IEffectControl[] effects) {
        foreach (var effect in effects) {
            controls.Add(effect.Name, effect);
        }        
    }

    public void Start() {        
        AddEffects(
            new ReverbControl(this)//,
            //new DelayControl(this),
            //new ChorusControl(this)
        );

        //effects.Add("reverb", GetComponent<AudioReverbFilter>());
        //effects.Add("delay", GetComponent<AudioEchoFilter>());
        //effects.Add("chorus", GetComponent<AudioChorusFilter>());
    }


    //public void Toggle(string name) => controls[name].Toggle();
}
