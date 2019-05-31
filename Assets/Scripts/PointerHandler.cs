using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerDownHandler {
    [System.Serializable]
    public class PointerHandlerEvent : UnityEvent<PointerEventData> { }

    public UnityEvent<PointerEventData> onPointerDown = new PointerHandlerEvent();

    public void OnPointerDown(PointerEventData eventData) {
        onPointerDown.Invoke(eventData);
    }
}
