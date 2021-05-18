using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class EffectControl<BaseEffect> where BaseEffect : Behaviour {
    public abstract string EffectName { get; }
    protected AudioManager Manager { get; }
    protected BaseEffect Effect { get; }
    protected Toggle Toggle { get; private set; }
    protected readonly Dictionary<string, Slider> sliders = new Dictionary<string, Slider>();    

    public EffectControl(AudioManager manager) {
        Manager = manager;
        Effect = manager.GetComponent<BaseEffect>();        
        LocateParameters();
        AddEvents();
    }

    

    //private void LocateParameters() {
    //    var parent = GameObject.Find(EffectName).transform;

    //    foreach (Transform childTransform in parent) {
    //        var child = childTransform.gameObject;
    //        var name = child.name;

    //        var label = childTransform.GetChild(0);
    //        var textComponent = label.GetComponent<Text>();
    //        textComponent.text = name;

    //        if (name == "Toggle") {
    //            Toggle = child.GetComponent<Toggle>();
    //            Toggle.onValueChanged.AddListener(OnToggle);
    //            textComponent.text = EffectName;
    //        } else {
    //            sliders.Add(name, child.GetComponent<Slider>());
    //            textComponent.text = name;
    //        }
    //    }
    //}

    private void LocateParameters() {
        Utils.ForeachChildOf(EffectName, child => {
            var name = child.name;

            var label = child.transform.GetChild(0);
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
        });
    }

    public virtual void OnToggle(bool isOn) => Effect.enabled = isOn;

    public abstract void AddEvents();
    protected void Slider(string sliderName, (float min, float max) range, UnityAction<float> setter) {
        var slider = sliders[sliderName];
        
        slider.minValue = range.min;
        slider.maxValue = range.max;

             
        slider.onValueChanged.AddListener(setter);

        slider.value = (range.min + range.max) / 2;   
    }    
}
