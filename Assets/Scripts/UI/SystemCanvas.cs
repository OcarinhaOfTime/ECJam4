using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemCanvas : MonoBehaviour {
    public ScaleButton quitButton;

    private void Start() {
        quitButton.onClick.AddListener(() => {
            PopupCanvas.instance.ShowOptionPopup("Exit to main menu?", () => { SceneManager.LoadScene(0); }, () => { });
        });
    }
}
