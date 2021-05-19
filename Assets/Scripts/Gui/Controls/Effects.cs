using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReverbControl : EffectUIControl<AudioReverbFilter> {
    public ReverbControl(AudioManager manager) : base(manager) { }
    public override string ControlGroup => "Reverb";
    protected override void AddEvents() {        
        
        Slider("Mix", (-2500, -500), value => Effect.reverbLevel = value);
        Slider("Decay", (0, 10), value => Effect.decayTime = value);
    }
}

public class DelayControl : EffectUIControl<AudioEchoFilter> {
    public DelayControl(AudioManager manager) : base(manager) { }
    public override string ControlGroup => "Delay";
    protected override void Initialize() {
        base.Initialize();
        EffectToggle.isOn = false;
    }
    protected override void AddEvents() {
        Slider("Mix", (0, 1), value => {
            Effect.wetMix = value;
            wetValue = value;
        });
        Slider("Rate", (100, 3000), _ => ChangeDelayTime());
        Slider("Decay", (0, 1), DecaySlider);

        Toggles["Sync"].onValueChanged.AddListener(_ => ChangeDelayTime());
    }

    public void ChangeDelayTime() {
        float sliderValue = Sliders["Rate"].value;

        if (Toggles["Sync"].isOn) {
            float k = (sliderValue - 100) / 580 - 2;
            Effect.delay = Manager.BeatInMilliseconds * (float)Math.Pow(2, (int)k);
        } else {
            Effect.delay = sliderValue;
        }
    }

    private float wetValue;
    protected override void OnToggle(bool isOn) {
        base.OnToggle(isOn);

        var decay = Sliders["Decay"];
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
        Effect.wetMix = wetValue;
    }
}

public class ChorusControl : EffectUIControl<AudioChorusFilter> {
    public ChorusControl(AudioManager manager) : base(manager) { }
    public override string ControlGroup => "Chorus";
    protected override void Initialize() {
        base.Initialize();
        EffectToggle.isOn = false;
    }
    protected override void AddEvents() {
        Slider("Mix", (0, 1), value => {
            Effect.wetMix1 = value;
            Effect.wetMix2 = value;
        });
        Slider("Rate", (1.1f, 7.4f), value => Effect.rate = value);
        Slider("Depth", (0, 0.22f), value => Effect.depth = value);
    }
}
