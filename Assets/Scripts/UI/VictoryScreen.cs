using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VUtils;

public class VictoryScreen : MonoBehaviour {
    public Image panel;
    public RectTransform mahou;
    public RectTransform item;
    public RectTransform gold;
    public PointerHandler pch;

    private GameObject canvas;
    private bool showing;
    private CanvasGroup cg;

    private void Start() {
        canvas = transform.GetChild(0).gameObject;
        cg = GetComponent<CanvasGroup>();
        canvas.SetActive(false);
        pch.onPointerDown.AddListener((e) => showing = false);
    }

    public Coroutine Show(int xp, string earned_item, int g) {
        return StartCoroutine(ShowRoutine(xp, earned_item, g));
    }

    private IEnumerator ShowRoutine(int xp, string earned_item, int g) {
        showing = true;
        panel.fillAmount = 0;
        cg.alpha = 1;
        canvas.SetActive(true);

        mahou.GetChild(2).GetComponent<TMPro.TMP_Text>().text = "" + xp;
        item.GetChild(1).GetComponent<TMPro.TMP_Text>().text = earned_item + " x1";
        gold.GetChild(2).GetComponent<TMPro.TMP_Text>().text = "$" + g;

        yield return this.LerpRoutine(.33f, CoTween.SmoothStop2, (t) => panel.fillAmount = t);

        while (showing)
            yield return null;

        yield return this.LerpRoutine(.25f, CoTween.SmoothStart2, (t) => cg.alpha = 1 - t);
        canvas.SetActive(false);
    }
}
