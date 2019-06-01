using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VUtils;

public class ScaleButton : MonoBehaviour, IPointerClickHandler {
    public UnityEvent onClick;
    private Vector3 originalSize;

    private void Start() {
        originalSize = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData) {
        StopAllCoroutines();
        this.PingPongRoutine(.2f, (t) => transform.localScale = Vector3.Lerp(originalSize, originalSize * 1.1f, t), onClick.Invoke);
    }
}