using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommoExtentions {
    public static Color SetAlpha(this Color color, float a) {
        var c = color;
        c.a = a;
        return c;
    }

    public static RectTransform GetRectTransform(this CanvasGroup cg) {
        return cg.transform as RectTransform;
    }
}
