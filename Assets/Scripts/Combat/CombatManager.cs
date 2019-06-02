using UnityEngine;
using UnityEngine.Events;

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
    public ScaleButton itens;
    public ScaleButton block;

    private SphereAttackManager sphereAttackManager;
    public float attackDuration = 5;
    public CombatManagerState state = CombatManagerState.Idle;
    public HUD hud;

    private float timer = 0;
    private string _status;
    private GameObject root;
    private IntersectionEvaluator evaluator;

    public float timeLefetNorm {
        get {
            return  1 - Mathf.Clamp01(timer / attackDuration);
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
    }

    public void Setup(Character player, Character enemy) {
        timer = 0;
        this.player = player;
        this.enemy = enemy;
        playerHolder.Setup(player.data);
        enemyHolder.Setup(enemy.data);
        buttons.SetActive(true);

        sphereAttackManager.Setup(playerHolder, enemyHolder);

        root.SetActive(true);
        hud.Setup(player, enemy);
    }

    public void Stop() {
        root.SetActive(false);
    }

    private void Update() {
        switch(state) {
            case CombatManagerState.PlayerAttack:
                PlayerAttack();
                break;
            case CombatManagerState.EnemyAttack:
                EnemyAttack();
                break;
        }
    }

    private void SetState(CombatManagerState state) {
        this.state = state;
        switch (state) {
            case CombatManagerState.PlayerAttack:
                buttons.SetActive(false);
                sphereAttackManager.ActivateAttack(OnPlayerAttackEvaluate);
                status = "player turn";
                break;
            case CombatManagerState.EnemyAttack:
                buttons.SetActive(false);
                sphereAttackManager.ActivateDefense(OnEnemyAttackEvaluate);
                break;

            case CombatManagerState.Idle:
                buttons.SetActive(true);
                status = "idle";
                break;
        }
    }

    private void OnPlayerAttackEvaluate() {
        if (!sphereAttackManager.attackConnect) {
            print("ATK MISSED");
            return;
        }

        int evalIndex = evaluator.EvaluateAttack(sphereAttackManager.attackLine, enemyHolder.col.ToRectange());
        int damage = Mathf.CeilToInt(player.data.strength * evaluator.atk_modifiers[evalIndex] + evalIndex);

        print(evaluator.atk_modifierLabels[evalIndex] + " attack " + damage);
        enemy.TakeDamage(damage);
        status = "player attacked";

        hud.ShowAttackModifier(evalIndex, damage, sphereAttackManager.attackLine.center);

        if(enemy.hp <= 0) {
            SetState(CombatManagerState.BattleOver);
            GameManager.instance.EndCombat();
        }
    }

    private void OnEnemyAttackEvaluate() {
        if (!sphereAttackManager.defenceConnected) {
            player.TakeDamage(enemy.data.strength);
            player.TakeDamage(enemy.data.strength);
            if (player.hp <= 0) {
                GameManager.instance.GameOver();
            }
            sphereAttackManager.StopDefense();
            SetState(CombatManagerState.Idle);
            return;
        }

        var def_line = sphereAttackManager.defenceLine;
        var atk_line = sphereAttackManager.enemyLine;

        int evalIndex = evaluator.EvaluateDefence(def_line, atk_line);
        int damage = Mathf.FloorToInt(enemy.data.strength * evaluator.def_modifiers[evalIndex]);

        print(evaluator.def_modifierLabels[evalIndex] + " block " + damage);

        player.TakeDamage(damage);
        hud.ShowAttackModifier(evalIndex, damage, sphereAttackManager.enemyLine.center);
        if (player.hp <= 0) {
            GameManager.instance.GameOver();
        }

        sphereAttackManager.StopDefense();
        SetState(CombatManagerState.Idle);
    }

    private void PlayerAttack() {
        if(timer > attackDuration) {
            timer = 0;
            sphereAttackManager.StopAttack();
            print("player turn finished");
            SetState(CombatManagerState.EnemyAttack);
            status = "Enemy turn";
            return;
        }

        timer += Time.deltaTime;
    }

    private void EnemyAttack() {
        
    }
}
