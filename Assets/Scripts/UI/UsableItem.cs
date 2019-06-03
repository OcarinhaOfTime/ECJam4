using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UsableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public TMP_Text nameTxt;
    public TMP_Text valueTxt;

    private Image background;
    public int id;

    private ItemsMenu im;

    private void Start() {
        im = GetComponentInParent<ItemsMenu>();
        background = transform.GetChild(0).GetComponent<Image>();
        background.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        SFXManager.instance.PlayClip(0);
        background.gameObject.SetActive(true);
        im.HoverItem(id);
    }

    public void OnPointerExit(PointerEventData eventData) {
        background.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        SFXManager.instance.PlayClip(1);
        background.gameObject.SetActive(false);
        im.UseItem(id);
    }

    public void UpdateValues(int id, string itemName, int itemCount) {
        this.id = id;
        nameTxt.text = itemName;
        valueTxt.text = "x" + itemCount;
    }
}
