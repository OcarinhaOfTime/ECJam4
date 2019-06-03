using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VUtils;

public class ScaleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public UnityEvent onClick;
    private Vector3 originalSize;

    private void Start() {
        originalSize = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData) {
        StopAllCoroutines();
        this.PingPongRoutine(.2f, (t) => transform.localScale = Vector3.Lerp(originalSize, originalSize * 1.2f, t), onClick.Invoke);
        SFXManager.instance.PlayClip(0);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale = originalSize * 1.1f;
        SFXManager.instance.PlayClip(1);
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale = originalSize;
    }
}