using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class EffectControl<BaseEffect> : IEffectControl
    where BaseEffect : Behaviour {

    protected Component Component { get; }
    protected BaseEffect Effect { get; }

    protected Dictionary<string, Behaviour> parameters = new Dictionary<string, Behaviour>() {
        ["toggle"] = null,
        ["wet"] = null
    };

    protected Toggle toggle;
    protected Slider wetSlider;

    public abstract string Name { get; }

    [Range(0, 1)]
    public float mix;

    public EffectControl(Component component) {
        Component = component;
        Effect = component.GetComponent<BaseEffect>();

        
        AssignUI(FindGroupUI());
    }

    private IEnumerable<GameObject> FindGroupUI() {
        var parent = GameObject.Find(Name).transform;

        //for (int childIndex = 0; childIndex < parent.childCount; childIndex++) {
        //}

        foreach (Transform childTransform in parent) {
            var child = childTransform.gameObject;
            yield return child;
        }
    }

    private void AssignUI(IEnumerable<GameObject> children) {
        foreach (var child in children) {
            var name = child.name;

            //if (!parameters.ContainsKey(name)) continue;

            switch (name) {
                case "toggle": {
                    toggle = child.GetComponent<Toggle>();
                    //parameters[name] = child.GetComponent<Toggle>();
                    break;
                }
                case "wet": {
                    wetSlider = child.GetComponent<Slider>();

                    //parameters[name] = child.GetComponent<Slider>();
                    break;
                }
            }
        }

        //((Toggle)parameters["toggle"]).onValueChanged += 

        toggle.onValueChanged.AddListener(Toggle);
        //wetSlider.onValueChanged.AddListener();
    }

    public void Toggle(bool value) => Effect.enabled = value;

    //public void Wet(float value) => Effect.;
}
