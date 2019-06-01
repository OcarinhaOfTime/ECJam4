using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VUtils;

public class GameManager : MonoBehaviour {
    public Image fader;
    public GameObject combatManager;
    public GameObject mainScene;
    public AsymptoticCamera cam;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCombat();
    }

    [ContextMenu("StartCombat")]
    public void StartCombat() {
        StartCoroutine(StartCombatRoutine());
    }

    private IEnumerator StartCombatRoutine() {
        yield return this.LerpRoutine(2, CoTween.SmoothStep, (t) => fader.color = Color.Lerp(Color.clear, Color.black, t));
        cam.enabled = false;
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        mainScene.SetActive(false);
        combatManager.SetActive(true);
        yield return this.LerpRoutine(1, CoTween.SmoothStep, (t) => fader.color = Color.Lerp(Color.clear, Color.black, 1-t));
    }

    public void EndCombat() {

    }
}
