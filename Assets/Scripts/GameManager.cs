using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VUtils;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public FaderCanvas fader;
    public CombatManager combatManager;
    public GameObject mainScene;
    public AsymptoticCamera cam;
    public PlayerController player;
    private Character playerCharacter;
    private Character enemy;
    public EventTrigger creditsTrigger;

    private void Awake() {
        instance = this;        
    }

    private void Start() {
        playerCharacter = player.GetComponent<Character>();
        creditsTrigger.onTriggerEnter.AddListener(RollCredits);
    }

    public void StartCombat(Character enemy) {
        this.enemy = enemy;
        player.controlling = false;
        StartCoroutine(StartCombatRoutine(enemy));
    }

    private IEnumerator StartCombatRoutine(Character enemy) {
        MusicManager.instance.FadeInOutMusic(1);
        yield return fader.Blink();
        yield return fader.FadeIn();
        cam.enabled = false;
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        mainScene.SetActive(false);
        combatManager.Setup(playerCharacter, enemy);
        yield return new WaitForSeconds(.33f);
        yield return fader.FadeOut();
    }

    public void StartBossCombat(Character enemy) {
        this.enemy = enemy;
        player.controlling = false;
        StartCoroutine(StartBossCombatRoutine(enemy));
    }

    private IEnumerator StartBossCombatRoutine(Character enemy) {
        MusicManager.instance.Play2PartSong(0);
        yield return fader.BannerFadeIn();
        yield return fader.FadeBlackIn();
        fader.BannerFadeOut();
        cam.enabled = false;
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        mainScene.SetActive(false);
        combatManager.Setup(playerCharacter, enemy);
        yield return new WaitForSeconds(.33f);
        yield return fader.FadeBlackOut();
    }

    public void EndCombat() {
        StartCoroutine(EndCombatRoutine());
    }

    private IEnumerator EndCombatRoutine() {
        yield return new WaitForSeconds(.5f);
        DataManager.instance.Save();
        yield return fader.FadeIn();
        cam.enabled = true;
        cam.JumpToTarget();
        mainScene.SetActive(false);
        combatManager.Stop();
        Destroy(enemy.gameObject);
        mainScene.SetActive(true);
        yield return new WaitForSeconds(.33f);
        yield return fader.FadeOut();
        player.controlling = true;

        MusicManager.instance.FadeInMusic(0);
    }

    public void GameOver() {
        DataManager.instance.Save();
        MusicManager.instance.FadeInOutMusic(3);
        fader.GameOverFade();

        this.ExecWhen(() => Input.anyKeyDown, () => { MusicManager.instance.FadeInOutMusic(0); SceneManager.LoadScene(0);});
    }

    public void RollCredits() {
        DataManager.instance.Save();
        StartCoroutine(RollCreditsRoutine());
    }

    private IEnumerator RollCreditsRoutine() {
        player.controlling = false;
        yield return fader.FadeBlackIn();
        SceneManager.LoadScene(2);
    }
}
