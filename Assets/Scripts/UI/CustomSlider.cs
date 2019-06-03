using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomSlider : MonoBehaviour, IPointerDownHandler {
    public float currentValue;
    private RectTransform rect;
    private Camera cam;
    private void Start() {
        rect = transform as RectTransform;
    }

    public void SetValue(float val) {
        currentValue = val;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, eventData.enterEventCamera, out localPoint);
        print(localPoint);
    }
}
