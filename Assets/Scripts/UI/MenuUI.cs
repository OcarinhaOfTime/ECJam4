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
            canvases[_current_index].gameObject.SetActive(false);
            _current_index = (value + canvases.Length) % canvases.Length;
            canvases[_current_index].gameObject.SetActive(true);
            canvases[_current_index].Appear();
        }
    }

    public CanvasUI[] canvases;
    private GameObject root;

    private bool isActive;

    private void Start() {
        root = transform.GetChild(0).gameObject;
        for (int i = 0; i < canvases.Length; i++) {
            canvases[i].gameObject.SetActive(false);
        }

        root.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isActive = !isActive;
            root.SetActive(isActive);
            ChangeMenu(0);
        }
    }

    

    public void ChangeMenu(int i) {
        current_index = current_index + i;
    }
}
