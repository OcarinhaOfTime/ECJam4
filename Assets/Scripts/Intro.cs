using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
    private void Update() {
        if (Input.anyKeyDown) {
            LoadGame();
        }

        if (Input.GetMouseButtonDown(0)) {
            LoadGame();
        }
    }

    private void LoadGame() {
        SceneManager.LoadScene(1);
    }
}
