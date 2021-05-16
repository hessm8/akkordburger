using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public void AddEffects() {
        new ReverbControl(this);
        new DelayControl(this);
        new ChorusControl(this);
    }

    public void Start() {        
        AddEffects();
    }
}
