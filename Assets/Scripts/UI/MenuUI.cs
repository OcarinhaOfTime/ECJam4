using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VUtils;

public class MenuUI : MonoBehaviour {
    private int _current_index;
    private int current_index {
        get {
            return _current_index;
        }
        set {
            canvases[_current_index].Hide();
            canvases[_current_index].gameObject.SetActive(false);
            _current_index = (value + canvases.Length) % canvases.Length;
            canvases[_current_index].gameObject.SetActive(true);
            canvases[_current_index].Appear();
        }
    }

    public CanvasUI[] canvases;
    private GameObject root;

    private bool isActive;
    private PlayerController playerController;

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        root = transform.GetChild(0).gameObject;
        for (int i = 0; i < canvases.Length; i++) {
            canvases[i].gameObject.SetActive(false);
        }

        root.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.instance.inCombat) {
            GameManager.instance.inMenu = true;
            isActive = !isActive;
            root.SetActive(isActive);
            if (isActive) {
                playerController.controlling = false;
                ChangeMenu(0);
            }
            else {
                GameManager.instance.inMenu = false;
                foreach (var c in canvases)
                    c.Hide();

                _current_index = 0;
                playerController.controlling = true;
            }
        }
    }

    

    public void ChangeMenu(int i) {
        current_index = current_index + i;
    }
}
