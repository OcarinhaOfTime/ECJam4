using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public TMP_Text log;
    public TMP_Text playerHP;
    public TMP_Text enemyHP;

    public Character player;
    public Character enemy;
    public CombatManager combatManager;
    public GameObject results_panel;
    public TMP_Text result;
    public Image timeLeftFill;

    private void Start() {
        UpdateHP(player, playerHP);
        UpdateHP(enemy, enemyHP);

        player.onTakeDamage.AddListener(() => UpdateHP(player, playerHP));
        enemy.onTakeDamage.AddListener(() => UpdateHP(enemy, enemyHP));

        player.onDie.AddListener(() => ActivateResultPanel("You lose"));
        enemy.onDie.AddListener(() => ActivateResultPanel("You win"));

        combatManager.onStatusChange.AddListener(() => log.text = combatManager.status);
    }

    private void UpdateHP(Character c, TMP_Text txt) {
        txt.text = c.hp + "/" + c.max_hp;
    }

    private void ActivateResultPanel(string txt) {
        results_panel.SetActive(true);
        result.text = txt;
    }

    private void Update() {
        timeLeftFill.fillAmount = combatManager.timeLefetNorm;
    }
}
