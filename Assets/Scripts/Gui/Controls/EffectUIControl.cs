using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class EffectUIControl<BaseEffect> : UIControl
    where BaseEffect : Behaviour {

    protected BaseEffect Effect { get; private set; }
    protected Toggle EffectToggle { get; private set; }
    public EffectUIControl(AudioManager manager) : base(manager) { }

    protected override void Initialize() {
        Effect = Manager.GetComponent<BaseEffect>();

        EffectToggle = Toggles["Toggle"];
        Toggles.Remove("Toggle");

        EffectToggle.onValueChanged.AddListener(OnToggle);

        GetLabel(EffectToggle.gameObject, out var textComp);
        textComp.text = ControlGroup;
    }

    protected virtual void OnToggle(bool isOn) => Effect.enabled = isOn;
}
