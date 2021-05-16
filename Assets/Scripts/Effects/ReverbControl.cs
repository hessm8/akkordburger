using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReverbControl : EffectControl<AudioReverbFilter> {
    public ReverbControl(Component component) : base(component) { }
    public override string EffectName => "Reverb";
    public override void AddEvents() {
        Slider("Decay", value => Effect.decayTime = value);     
    }
}
