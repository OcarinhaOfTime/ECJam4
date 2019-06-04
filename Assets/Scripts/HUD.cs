using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VUtils;

public class HUD : MonoBehaviour {
    public TMP_Text log;
    public TMP_Text playerHP;
    public TMP_Text enemyHP;

    public TMP_Text playerName;
    public TMP_Text enemyName;

    private Character player;
    private Character enemy;
    public CombatManager combatManager;
    public Image timeLeftFill;
    private IntersectionEvaluator evaluator;

    public RectTransform impact;
    public TMP_Text damageTxt;
    public TMP_Text modifierTxt;
    public TMP_Text turnInfo;

    public bool showingModifiers = false;

    private void Start() {
        combatManager.onStatusChange.AddListener(UpdateValues);
        evaluator = combatManager.GetComponent<IntersectionEvaluator>();
    }

    public void Setup(Character player, Character enemy) {
        turnInfo.color.SetAlpha(0);
        this.player = player;
        this.enemy = enemy;
        impact.localScale = modifierTxt.transform.localScale = Vector3.zero;
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
        txt.text = c.hp + "/" + c.max_hp;
    }

    public Coroutine ShowAttackModifier(int evalIndex, int damage, Vector2 attackPosition) {
        damageTxt.text = "" + damage;
        modifierTxt.text = evaluator.atk_modifierLabels[evalIndex];
        impact.position = attackPosition;
        modifierTxt.transform.position = impact.position + Vector3.up * 1f + Vector3.right * -.25f;
        return StartCoroutine(ShowModifiers());
    }

    public Coroutine ShowAttackModifier2(string modif, int damage, Vector2 attackPosition) {
        damageTxt.text = "" + damage;
        modifierTxt.text = modif;
        impact.position = attackPosition;
        modifierTxt.transform.position = impact.position + Vector3.up * 1f + Vector3.right * -.25f;
        return StartCoroutine(ShowModifiers());
    }

    public Coroutine ShowDefenceModifier(int evalIndex, int damage, Vector2 attackPosition) {
        damageTxt.text = "" + damage;
        modifierTxt.text = evaluator.def_modifierLabels[evalIndex] + " block";
        impact.position = attackPosition;
        modifierTxt.transform.position = impact.position + Vector3.up + Vector3.right * .25f;
        return StartCoroutine(ShowModifiers());
    }

    public Coroutine ShowTurnInfo(string nam) {
        SFXManager.instance.PlayClip(6);
        turnInfo.text = nam + "\nAttacks";
        return StartCoroutine(ShowTurnInfoRoutine());
    }

    private IEnumerator ShowTurnInfoRoutine() {
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => {
            turnInfo.color = turnInfo.color.SetAlpha(t);
            turnInfo.rectTransform.anchoredPosition = Vector2.Lerp(Vector2.right * 300, Vector2.zero, t);
        });

        yield return new WaitForSeconds(1);

        yield return this.LerpRoutine(.1f, CoTween.SmoothStep, (t) => {
            turnInfo.color = turnInfo.color.SetAlpha(1-t);
            turnInfo.rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, Vector2.right * -300, t);
        });
    }

    private IEnumerator ShowModifiers() {
        showingModifiers = true;
        yield return this.LerpRoutine(.1f, CoTween.SmoothStep, (t) => {
            impact.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        });

        yield return this.LerpRoutine(.1f, CoTween.SmoothStep, (t) => {
            modifierTxt.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        });

        yield return new WaitForSeconds(1);

        yield return this.LerpRoutine(.1f, CoTween.SmoothStep, (t) => {
            impact.localScale = modifierTxt.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1-t);
        });

        showingModifiers = false;
    }

    private void Update() {
        timeLeftFill.fillAmount = combatManager.timeLefetNorm;
    }
}
