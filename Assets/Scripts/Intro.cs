using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VUtils;

public class Intro : MonoBehaviour {
    public Graphic background;
    public Graphic title1;
    public Graphic title2;
    public Image menu;
    public Image credits;

    public PointerClickHandler newGameButton;
    public PointerClickHandler loadGameButton;
    public PointerClickHandler creditsButton;
    public PointerClickHandler exitButton;

    private bool loaded;

    private bool onCredits;

    private bool anyInput {
        get {
            return Input.anyKeyDown || Input.GetMouseButtonDown(0);
        }
    }

    private IEnumerator Start() {
        newGameButton.onClick.AddListener(NewGame);
        loadGameButton.onClick.AddListener(LoadGame);
        creditsButton.onClick.AddListener(Credits);
        exitButton.onClick.AddListener(Exit);

        var bgcolor = background.color;
        background.color = Color.clear;

        menu.fillAmount = 0;

        var t1start = title1.rectTransform.anchoredPosition;
        var t2start = title2.rectTransform.anchoredPosition;
        var tcolor = title1.color;
        title1.color = title2.color = Color.clear;
        menu.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        yield return this.LerpRoutine(.5f, CoTween.SmootherStep, (t) => background.color = Color.Lerp(Color.clear, bgcolor, t));

        this.LerpRoutine(.5f, CoTween.SmoothStop2, (t) => {
            title1.color = Color.Lerp(Color.clear, tcolor, t);
            title1.rectTransform.anchoredPosition = Vector2.Lerp(t1start-Vector2.right * 500, t1start, t);
        });

        yield return new WaitForSeconds(.25f);

        yield return this.LerpRoutine(.5f, CoTween.SmoothStop2, (t) => {
            title2.color = Color.Lerp(Color.clear, tcolor, t);
            title2.rectTransform.anchoredPosition = Vector2.Lerp(t2start - Vector2.right * 500, t2start, t);
        });

        yield return new WaitForSeconds(.25f);
        loaded = true;        
    }

    private void Update() {
        if (!loaded)
            return;
        if (anyInput && !menu.gameObject.activeSelf) {
            if (onCredits) {
                StartCoroutine(CreditsOffRoutine());
            } else {
                menu.gameObject.SetActive(true);
                this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => menu.fillAmount = t);
            }            
        }
    }

    public void NewGame() {
        SceneManager.LoadScene(1);
    }

    public void LoadGame() {
        SceneManager.LoadScene(1);
    }

    public void Credits() {
        StartCoroutine(CreditsRoutine());
    }

    private IEnumerator CreditsRoutine() {
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => menu.fillAmount = 1 - t);
        menu.gameObject.SetActive(false);
        onCredits = true;
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => credits.fillAmount = t);
    }

    private IEnumerator CreditsOffRoutine() {
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => credits.fillAmount = 1-t);
        onCredits = false;
        menu.gameObject.SetActive(true);
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => menu.fillAmount = t);
    }

    public void Exit() {
        Application.Quit();
    }
}
