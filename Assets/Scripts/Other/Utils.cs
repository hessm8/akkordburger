using System;
using UnityEngine;

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

    public static bool HasComponent<T>(this GameObject obj, out T component) where T : Component {
        component = obj.GetComponent<T>();
        return component != null;
    }
}

public static class General {
    internal static AudioManager AudioManager;
    internal static Color KeyColor => new Color(0.196f, 0.75f, 0.58f);
}

