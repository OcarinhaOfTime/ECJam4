using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusItem : MonoBehaviour, IPointerEnterHandler {
    public ScaleButton leftButton;
    public ScaleButton rightButton;
    public TMP_Text nameTxt;
    public TMP_Text valueTxt;
    private StatusMenu sm;

    private void Start() {
        sm = GetComponentInParent<StatusMenu>();
        //leftButton.onClick.AddListener(() => { sm.OnStatusClick(-1, nameTxt.text); });
        rightButton.onClick.AddListener(() => { sm.OnStatusClick(+1, nameTxt.text); });
    }

    public void UpdateValues(string nameT, int val) {
        nameTxt.text = nameT;
        valueTxt.text = "" + val;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        sm.OnStatusHover(nameTxt.text);
    }
}
