using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VUtils;

public class Ending : MonoBehaviour {
    public Image background;
    public Image creditsBanner;
    public CanvasGroup line1;
    public CanvasGroup line2;
    public CanvasGroup line3;

    private bool loaded;

    private IEnumerator Start() {
        var bgcolor = background.color;
        background.color = Color.clear;

        creditsBanner.fillAmount = 0;

        var t1start = line1.GetRectTransform().anchoredPosition;
        var t2start = line2.GetRectTransform().anchoredPosition;
        var t3start = line3.GetRectTransform().anchoredPosition;

        line1.alpha = line2.alpha = line3.alpha = 0;

        yield return new WaitForSeconds(1);
        yield return this.LerpRoutine(.5f, CoTween.SmootherStep, (t) => background.color = Color.Lerp(Color.clear, bgcolor, t));

        yield return new WaitForSeconds(1);

        yield return this.LerpRoutine(.5f, CoTween.SmootherStep, (t) => creditsBanner.fillAmount = t);

        this.LerpRoutine(1, CoTween.SmoothStop2, (t) => {
            line1.alpha = t;
            line1.GetRectTransform().anchoredPosition = Vector2.Lerp(t1start - Vector2.right * 500, t1start, t);
        });

        yield return new WaitForSeconds(1f);

        this.LerpRoutine(1f, CoTween.SmoothStop2, (t) => {
            line2.alpha = t;
            line2.GetRectTransform().anchoredPosition = Vector2.Lerp(t2start - Vector2.right * 500, t2start, t);
        });

        yield return new WaitForSeconds(1f);

        this.LerpRoutine(1, CoTween.SmoothStop2, (t) => {
            line3.alpha = t;
            line3.GetRectTransform().anchoredPosition = Vector2.Lerp(t3start - Vector2.right * 500, t3start, t);
        });

        yield return new WaitForSeconds(.25f);
        loaded = true;
    }

    private bool anyInput {
        get {
            return Input.anyKeyDown || Input.GetMouseButtonDown(0);
        }
    }

    private void Update() {
        if (!loaded)
            return;
        if (anyInput) {
            PopupCanvas.instance.ShowOptionPopup("Go back to main menu?", () => { SceneManager.LoadScene(0); }, () => { });
        }
    }
}
