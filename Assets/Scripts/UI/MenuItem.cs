using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private Image background;

    private void Start() {
        background = transform.GetChild(0).GetComponent<Image>();
        background.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        SFXManager.instance.PlayClip(0);
        background.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        background.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        SFXManager.instance.PlayClip(1);
        background.gameObject.SetActive(false);
    }
}
