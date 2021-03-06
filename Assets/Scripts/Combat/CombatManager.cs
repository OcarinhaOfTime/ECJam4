﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VUtils;

public class CombatManager : MonoBehaviour {
    public enum CombatManagerState {
        Idle,
        PlayerAttack,
        EnemyAttack,
        BattleOver
    }

    private Character player;
    private Character enemy;

    public CharacterHolder playerHolder;
    public CharacterHolder enemyHolder;

    public GameObject buttons;

    public ScaleButton attack;
    public ScaleButton items;
    public ScaleButton options;

    private SphereAttackManager sphereAttackManager;
    public CombatManagerState state = CombatManagerState.Idle;
    public HUD hud;
    public VictoryScreen victoryScreen;

    public CanvasUI itemsUI;
    public PointerClickHandler itemsUIClickHandler;

    public CanvasUI systemUI;
    public PointerClickHandler systemUIClickHandler;

    private string _status;
    private GameObject root;
    private IntersectionEvaluator evaluator;

    private float playerAttackTimer = 0;
    private float playerAttackDuration = 1;

    public float timeLefetNorm {
        get {
            return  Mathf.Clamp01(1 - playerAttackTimer / playerAttackDuration);
        }
    }
    public string status {
        get {
            return _status;
        }

        private set {
            _status = value;
            onStatusChange.Invoke();
        }
    }

    public UnityEvent onStatusChange;
    public UnityEvent onCombatEnd;
    public UnityEvent onSetup;

    private void Start() {
        status = "idle";
        root = transform.GetChild(0).gameObject;
        sphereAttackManager = GetComponent<SphereAttackManager>();
        attack.onClick.AddListener(() => SetState(CombatManagerState.PlayerAttack));
        evaluator = GetComponent<IntersectionEvaluator>();

        items.onClick.AddListener(itemsUI.Appear);
        itemsUIClickHandler.onClick.AddListener(itemsUI.Hide);
        itemsUI.GetComponent<ItemsMenu>().onItemUsed.AddListener(OnItemUsed);

        options.onClick.AddListener(systemUI.Appear);
        systemUIClickHandler.onClick.AddListener(systemUI.Hide);
    }

    private void OnItemUsed() {
        status = "used item";
        itemsUI.Hide();
        SetState(CombatManagerState.EnemyAttack);
    }

    public void Setup(Character player, Character enemy) {
        this.player = player;
        this.enemy = enemy;
        playerHolder.Setup(player.data);
        enemyHolder.Setup(enemy.data);
        buttons.SetActive(true);
        enemyHolder.animator.enabled = true;
        enemyHolder.alpha = 1;

        sphereAttackManager.Setup(playerHolder, enemyHolder);

        root.SetActive(true);
        hud.Setup(player, enemy);
    }

    public void Stop() {
        root.SetActive(false);
    }

    private void SetState(CombatManagerState state) {
        this.state = state;
        StartCoroutine(SetStateRoutine());
    }

    private IEnumerator SetStateRoutine() {
        switch (state) {
            case CombatManagerState.PlayerAttack:
                yield return SetPlayerAttack();
                break;
            case CombatManagerState.EnemyAttack:
                yield return SetEnemyAttack();
                break;

            case CombatManagerState.Idle:
                yield return this.ExecWhen(() => !hud.showingModifiers, () => {
                    buttons.SetActive(true);
                    status = "idle";
                });
                
                break;

            case CombatManagerState.BattleOver:
                yield return BattleOverRoutine();
                break;
        }

        yield return null;
    }

    private IEnumerator SetPlayerAttack() {
        buttons.SetActive(false);
        yield return hud.ShowTurnInfo(player.data.characterName);
        sphereAttackManager.ActivateAttack(OnPlayerAttackEvaluate);
        status = "player turn";

        print("player attacking");
        playerAttackTimer = 0;
        playerAttackDuration = player.data.agility * .5f;

        yield return new WaitForSeconds(3 + player.data.agility * .33f);

        sphereAttackManager.StopAttack();
        print("player turn finished");
        status = "Enemy turn";
        yield return this.ExecWhen(() => !hud.showingModifiers, () => {
            SetState(CombatManagerState.EnemyAttack);
        });
    }

    private IEnumerator SetEnemyAttack() {
        buttons.SetActive(false);
        yield return hud.ShowTurnInfo(enemy.data.characterName);
        sphereAttackManager.ActivateDefense(OnEnemyAttackEvaluate);
    }

    private void OnPlayerAttackEvaluate() {
        if (!sphereAttackManager.attackConnect) {
            print("ATK MISSED");
            hud.ShowAttackModifier2("Missed", 0, enemyHolder.transform.position);
            return;
        }

        if(Random.value * 100 <= player.data.luck) {
            int damage = player.data.strength * 5;

            print("lucky attack " + damage);
            enemy.TakeDamage(damage);
            status = "player attacked";

            hud.ShowAttackModifier2("Lucky", damage, sphereAttackManager.attackLine.center);
        } else {
            int evalIndex = evaluator.EvaluateAttack(sphereAttackManager.attackLine, enemyHolder.col.ToRectange());
            int damage = Mathf.CeilToInt(player.data.strength * evaluator.atk_modifiers[evalIndex] + evalIndex);

            print(evaluator.atk_modifierLabels[evalIndex] + " attack " + damage);
            enemy.TakeDamage(damage);
            status = "player attacked";

            hud.ShowAttackModifier(evalIndex, damage, sphereAttackManager.attackLine.center);
        }

        

        if(enemy.hp <= 0) {
            StopAllCoroutines();
            sphereAttackManager.StopAttack();
            SetState(CombatManagerState.BattleOver);
        }
    }

    private IEnumerator BattleOverRoutine() {
        enemyHolder.animator.enabled = false;
        MusicManager.instance.FadeInOutMusic(2, .25f, false);

        yield return this.LerpRoutine(1, CoTween.SmoothStep, (t) => enemyHolder.alpha = 1 - t);

        player.data.gold += enemy.data.gold;
        player.data.xp += enemy.data.xp;
        var item = DataManager.instance.inventoryManager.randomItem;
        DataManager.instance.inventoryManager.AddItem(item);

        yield return victoryScreen.Show(enemy.data.xp, DataManager.instance.inventoryManager.items[item], enemy.data.gold);
        GameManager.instance.EndCombat();
    }

    private void OnEnemyAttackEvaluate() {
        int evalIndex = 0;

        if (sphereAttackManager.defenceConnected) {
            var def_line = sphereAttackManager.defenceLine;
            var atk_line = sphereAttackManager.enemyLine;

            evalIndex = evaluator.EvaluateDefence(def_line, atk_line);
        }
        

        int damage = Mathf.FloorToInt(enemy.data.strength * evaluator.def_modifiers[evalIndex]);

        print(evaluator.def_modifierLabels[evalIndex] + " block " + damage);

        player.TakeDamage(damage);
        hud.ShowDefenceModifier(evalIndex, damage, sphereAttackManager.enemyLine.center);
        if (player.hp <= 0) {
            GameManager.instance.GameOver();
        }

        sphereAttackManager.StopDefense();
        SetState(CombatManagerState.Idle);
    }
}
