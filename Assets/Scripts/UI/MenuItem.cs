using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Image background;

    private void Start() {
        background = transform.GetChild(0).GetComponent<Image>();
        background.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        background.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        background.gameObject.SetActive(false);
    }
}
