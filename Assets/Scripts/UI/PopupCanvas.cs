using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupCanvas : MonoBehaviour {
    public static PopupCanvas instance;

    private void Awake() {
        instance = this;
    }

    public ImageButton optionPopupYesButton;
    public ImageButton optionPopupNoButton;
    public TMP_Text optionPopupText;
    public GameObject optionPopupPanel;

    public ImageButton notificationPopupButton;
    public TMP_Text notificationPopupText;
    public GameObject notificationPopup;

    public void ShowOptionPopup(string text, Action onYes, Action onNo) {
        optionPopupText.text = text;

        optionPopupYesButton.onClick.RemoveAllListeners();
        optionPopupYesButton.onClick.AddListener(() => onYes());
        optionPopupYesButton.onClick.AddListener(() => optionPopupPanel.SetActive(false));

        optionPopupNoButton.onClick.RemoveAllListeners();
        optionPopupNoButton.onClick.AddListener(() => onNo());
        optionPopupNoButton.onClick.AddListener(() => optionPopupPanel.SetActive(false));

        optionPopupPanel.SetActive(true);
    }

    public void ShowNotificationPopup(string text, Action onPress) {
        notificationPopupText.text = text;

        notificationPopupButton.onClick.RemoveAllListeners();
        notificationPopupButton.onClick.AddListener(() => onPress());
        notificationPopupButton.onClick.AddListener(() => notificationPopup.SetActive(false));

        notificationPopup.SetActive(true);
    }
}
