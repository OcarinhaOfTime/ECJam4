using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VUtils;

public class FaderCanvas : MonoBehaviour {
    public Image fader;

    public RectTransform banner;
    public CanvasGroup portrait;
    public RawImage bars;
    private Vector2 bannerOriginalSize;
    private Vector2 bannerOffSize;

    private Vector2 portraitOriginalPos;
    private Vector2 portraitOffPos;

    private Color barsColor;

    public Image blackPanel;
    public RectTransform gameOverTxt;

    private void Start() {
        bannerOriginalSize = banner.sizeDelta;
        bannerOriginalSize.y = 300;
        bannerOffSize = new Vector2(bannerOriginalSize.x, 0);
        portraitOriginalPos = portrait.GetComponent<RectTransform>().anchoredPosition;
        portraitOffPos = portrait.GetComponent<RectTransform>().anchoredPosition - Vector2.right * 500;

        banner.sizeDelta = bannerOffSize;
        portrait.alpha = 0;
        barsColor = bars.color;
        bars.color = Color.clear;
    }

    public Coroutine FadeIn() {
        return this.LerpRoutine(1, CoTween.SmoothStart2, (t) => fader.color = Color.Lerp(Color.clear, Color.black, t));
    }

    public Coroutine FadeOut() {
        return this.LerpRoutine(1, CoTween.SmoothStop2, (t) => fader.color = Color.Lerp(Color.clear, Color.black, 1-t));
    }

    public Coroutine FadeBlackIn() {
        return this.LerpRoutine(1, CoTween.SmoothStart2, (t) => blackPanel.color = Color.Lerp(Color.clear, Color.black, t));
    }

    public Coroutine FadeBlackOut() {
        return this.LerpRoutine(1, CoTween.SmoothStop2, (t) => blackPanel.color = Color.Lerp(Color.clear, Color.black, 1 - t));
    }


    public Coroutine BannerFadeIn() {
        return StartCoroutine(BannerAnimation());
    }
    public void BannerFadeOut() {
        banner.sizeDelta = bannerOffSize;
    }

    public IEnumerator BannerAnimation() {
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => banner.sizeDelta = Vector2.Lerp(bannerOffSize, bannerOriginalSize, t));
        yield return this.LerpRoutine(1, CoTween.SmoothStop3, (t) => {
            bars.color = Color.Lerp(Color.clear, barsColor, t);
            portrait.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(portraitOffPos, portraitOriginalPos, t);
            portrait.alpha = t * 2;
        });

        yield return new WaitForSeconds(1);
        //yield return this.LerpRoutine(.5f, CoTween.SmoothStart2, (t) => portrait.rectTransform.anchoredPosition = Vector2.Lerp(portraitOriginalPos, portraitOriginalPos + Vector2.right * 500, t));
    }

    public Coroutine GameOverFade() {
        return StartCoroutine(GameOverFadeRoutine());
    }

    public IEnumerator GameOverFadeRoutine() {
        yield return this.LerpRoutine(1f, CoTween.SmoothStep, (t) => blackPanel.color = Color.Lerp(Color.clear, Color.black, t));
        yield return this.LerpRoutine(1f, CoTween.SmoothStep, (t) => gameOverTxt.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, t));
    }
}
