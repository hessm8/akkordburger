using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class Utils {    
    public static void ForeachChildOf(string parentName, Action<GameObject> action) {
        var parent = GameObject.Find(parentName).transform;

        foreach (Transform childTransform in parent) {
            var child = childTransform.gameObject;
            action(child);
        }
    }

    public static bool In(this int num, float left, float right) {
        return num > left && num < right;
    }
}

public static class Akkordburger {
    internal static AudioManager AudioManager;
}

