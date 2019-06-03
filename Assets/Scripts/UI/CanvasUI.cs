using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VUtils;

public class CanvasUI : MonoBehaviour {
    public ScaleButton leftButton;
    public ScaleButton rightButton;

    public Image[] fills;
    private GameObject canvas;

    public float appear_duration = 1;
    public float appear_interval = 1;

    private MenuUI menuUI;

    public UnityEvent onAppear;

    private void Awake() {
        menuUI = GetComponentInParent<MenuUI>();
        leftButton.onClick.AddListener(() => menuUI.ChangeMenu(-1));
        rightButton.onClick.AddListener(() => menuUI.ChangeMenu(1));
        canvas = transform.GetChild(0).gameObject;
    }

    private void Start() {
        foreach (var fill in fills) {
            fill.fillAmount = 0;
        }
    }

    public void Appear() {
        onAppear.Invoke();
        canvas.SetActive(true);
        StartCoroutine(AppearRoutine());
    }

    public void Hide() {
        foreach (var fill in fills) {
            fill.fillAmount = 0;
        }
        canvas.SetActive(false);
    }

    public IEnumerator AppearRoutine() {
        foreach (var fill in fills) {
            this.LerpRoutine(appear_duration, CoTween.SmootherStep, (t) => fill.fillAmount = t);
            yield return new WaitForSeconds(appear_interval);
        }
        //yield return this.LerpRoutine(appear_duration * .5f, CoTween.SmoothStep, (t) => help.alpha = t);
        yield return new WaitForSeconds(appear_duration);
    }

    [ContextMenu("Disappear")]
    public void Disappear() {
        StartCoroutine(DisappearRoutine());
    }

    public IEnumerator DisappearRoutine() {
        //yield return this.LerpRoutine(appear_duration, CoTween.SmoothStep, (t) => help.alpha = 1 - t);
        foreach (var fill in fills) {
            this.LerpRoutine(appear_duration * .25f, CoTween.SmootherStep, (t) => fill.fillAmount = 1 - t);
            yield return new WaitForSeconds(appear_interval * .25f);
        }

        yield return new WaitForSeconds(.25f);
        canvas.SetActive(false);
    }
}
