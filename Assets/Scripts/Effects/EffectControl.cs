using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class EffectControl<BaseEffect> : IEffectControl
    where BaseEffect : Behaviour {
    public abstract string EffectName { get; }
    protected Component Component { get; }
    protected BaseEffect Effect { get; }
    protected Toggle Toggle { get; private set; }
    private readonly Dictionary<string, Slider> sliders = new Dictionary<string, Slider>();    

    public EffectControl(Component component) {
        Component = component;
        Effect = component.GetComponent<BaseEffect>();        
        LocateParameters();
        AddEvents();
    }

    private void LocateParameters() {
        var parent = GameObject.Find(EffectName).transform;

        foreach (Transform childTransform in parent) {
            var child = childTransform.gameObject;
            var name = child.name;

            var label = childTransform.GetChild(0);
            var textComponent = label.GetComponent<Text>();
            textComponent.text = name;

            if (name == "Toggle") {
                Toggle = child.GetComponent<Toggle>();
                Toggle.onValueChanged.AddListener(OnToggle);
                textComponent.text = EffectName;
            } else {
                sliders.Add(name, child.GetComponent<Slider>());
                textComponent.text = name;
            }
        }
    }

    public void OnToggle(bool value) => Effect.enabled = value;

    public abstract void AddEvents();
    protected void Slider(string sliderName, UnityAction<float> setter) {
        sliders[sliderName].onValueChanged.AddListener(setter);
    }
    
}
