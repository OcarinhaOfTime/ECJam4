using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public TMP_Text log;
    public TMP_Text playerHP;
    public TMP_Text enemyHP;

    public TMP_Text playerName;
    public TMP_Text enemyName;

    private Character player;
    private Character enemy;
    public CombatManager combatManager;
    public GameObject results_panel;
    public TMP_Text result;
    public Image timeLeftFill;

    private void Start() {
        combatManager.onStatusChange.AddListener(UpdateValues);
    }

    public void Setup(Character player, Character enemy) {
        this.player = player;
        this.enemy = enemy;
        UpdateValues();
    }

    private void UpdateValues() {
        UpdateHP(player, playerHP);
        UpdateHP(enemy, enemyHP);
        playerName.text = player.data.characterName;
        enemyName.text = enemy.data.characterName;
        log.text = combatManager.status;
    }

    private void UpdateHP(Character c, TMP_Text txt) {
        txt.text = c.hp + "/" + c.data.vitality;
    }

    private void ActivateResultPanel(string txt) {
        results_panel.SetActive(true);
        result.text = txt;
    }

    private void Update() {
        timeLeftFill.fillAmount = combatManager.timeLefetNorm;
    }
}
