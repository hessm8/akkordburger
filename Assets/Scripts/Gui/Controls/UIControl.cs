using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UIControl {
    public abstract string ControlGroup { get; }
    protected AudioManager Manager { get; }
    protected Dictionary<string, Toggle> Toggles { get; }
    protected Dictionary<string, Slider> Sliders { get; }
    protected virtual void Locate(GameObject child) { }
    protected virtual void Initialize() { }
    protected abstract void AddEvents();
    public UIControl(AudioManager manager) {
        Manager = manager;

        Toggles = new Dictionary<string, Toggle>();
        Sliders = new Dictionary<string, Slider>();

        LocateParameters();

        Initialize();

        AddEvents();
    }    

    private void LocateParameters() {
        Utils.ForeachChildOf(ControlGroup, child => {
            var name = child.name;

            GetLabel(child, out var textComponent);

            if (child.HasComponent<Toggle>(out var toggle)) {
                Toggles.Add(name, toggle);
            } else if (child.HasComponent<Slider>(out var slider)) {
                Sliders.Add(name, child.GetComponent<Slider>());
                textComponent.text = name;
            } else Locate(child);

        });
    }   

    protected void GetLabel(GameObject obj, out Text textComponent) {
        var label = obj.transform.GetChild(0);
        textComponent = label.GetComponent<Text>();
    }    
    protected void Slider(string sliderName, (float min, float max) range, UnityAction<float> setter) {
        var slider = Sliders[sliderName];

        slider.minValue = range.min;
        slider.maxValue = range.max;

        slider.onValueChanged.AddListener(setter);

        slider.value = (range.min + range.max) / 2;
    }
}