using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public float threshold = .2f;
    public UnityEvent onClick = new UnityEvent();

    private float clickTime;

    public void OnPointerDown(PointerEventData eventData) {
        clickTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (Time.time - clickTime < threshold)
            onClick.Invoke();
    }
}
