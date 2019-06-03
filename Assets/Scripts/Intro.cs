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
    private IEnumerator Start() {
        var bgcolor = background.color;
        background.color = Color.clear;

        //var t1start

        yield return new WaitForSeconds(1);
        yield return this.LerpRoutine(1, CoTween.SmootherStep, (t) => background.color = Color.Lerp(Color.clear, bgcolor, t));

    }

    private void Update() {
        if (Input.anyKeyDown) {
            //LoadGame();
        }

        if (Input.GetMouseButtonDown(0)) {
            //LoadGame();
        }
    }

    private void LoadGame() {
        SceneManager.LoadScene(1);
    }
}
