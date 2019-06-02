using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleLayoutGroup : LayoutGroup {
    public Vector2 center;
    public float size = 100;
    public float spacing = 0;
    public float angle;
    public override void CalculateLayoutInputVertical() {
        RecalculateLayout();
    }

    public override void SetLayoutHorizontal() {
        RecalculateLayout();
    }

    public override void SetLayoutVertical() {
        RecalculateLayout();
    }

    private void RecalculateLayout() {
        Vector2 up = Quaternion.Euler(0, 0, angle) * Vector2.up;
        var origin = center + up * rectTransform.childCount * size * .5f;
        for(int i=0; i<rectTransform.childCount; i++) {
            var child = rectTransform.GetChild(i) as RectTransform;
            child.anchoredPosition = origin - up * i * size;
        }
    }
}