using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReverbControl : EffectControl<AudioReverbFilter> {
    public ReverbControl(AudioManager manager) : base(manager) { }
    public override string EffectName => "Reverb";
    public override void AddEvents() {        
        Slider("Mix", (-2500, -500), value => Effect.reverbLevel = value);
        Slider("Decay", (0, 10), value => Effect.decayTime = value);
    }
}

public class DelayControl : EffectControl<AudioEchoFilter> {
    public DelayControl(AudioManager manager) : base(manager) { }
    public override string EffectName => "Delay";
    public override void AddEvents() {
        Slider("Mix", (0, 1), value => {
            Effect.wetMix = value;
            wet = value;
        });
        Slider("Rate", (100, 3000), value => Effect.delay = value);
        Slider("Decay", (0, 1), DecaySlider);
    }

    private float wet;
    public override void OnToggle(bool isOn) {
        base.OnToggle(isOn);

        var decay = sliders["Decay"];
        if (isOn) {           
            Manager.StartCoroutine(AdjustWet());            
            decay.onValueChanged.AddListener(DecaySlider);
            decay.value = 0.5f;
        } else {
            decay.onValueChanged.RemoveListener(DecaySlider);
            Effect.decayRatio = 0;
            Effect.wetMix = 0;
        }
    }
    private void DecaySlider(float value) => Effect.decayRatio = value;
    private IEnumerator AdjustWet() {
        yield return new WaitForSeconds(0.8f);
        Effect.wetMix = wet;
    }
}

public class ChorusControl : EffectControl<AudioChorusFilter> {
    public ChorusControl(AudioManager manager) : base(manager) { }
    public override string EffectName => "Chorus";
    public override void AddEvents() {
        Slider("Mix", (0, 1), value => {
            Effect.wetMix1 = value;
            Effect.wetMix2 = value;
        });
        Slider("Rate", (1.1f, 7.4f), value => Effect.rate = value);
        Slider("Depth", (0, 0.22f), value => Effect.depth = value);
    }
}
