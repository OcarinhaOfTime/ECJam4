using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour {
    public enum CombatManagerState {
        Idle,
        PlayerAttack,
        EnemyAttack
    }

    public Character player;
    public Character enemy;

    public ScaleButton attack;
    public ScaleButton itens;
    public ScaleButton block;

    private SphereAttackManager sphereAttackManager;
    public float attackDuration = 5;
    public CombatManagerState state = CombatManagerState.Idle;

    private float timer = 0;
    private string _status;
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

    private void Start() {
        status = "idle";
        sphereAttackManager = GetComponent<SphereAttackManager>();
        attack.onClick.AddListener(() => SetState(CombatManagerState.PlayerAttack));
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

    public void SetState(CombatManagerState state) {
        this.state = state;
        switch (state) {
            case CombatManagerState.PlayerAttack:
                sphereAttackManager.ActivateAttack(enemy.boxCollider, OnPlayerAttack);
                status = "player turn";
                break;
            case CombatManagerState.EnemyAttack:
                sphereAttackManager.ActivateAttack(player.boxCollider, OnEnemyAttack);
                break;
        }
    }

    private void OnPlayerAttack() {
        print("on player attack");
        enemy.TakeDamage(player.atk);
    }

    private void OnEnemyAttack() {
        player.TakeDamage(enemy.atk);
    }

    private void PlayerAttack() {
        if(timer > attackDuration) {
            state = CombatManagerState.Idle;
            timer = 0;
            sphereAttackManager.StopAttack();
            print("player turn finished");
            status = "idle";
            return;
        }

        timer += Time.deltaTime;
    }

    private void EnemyAttack() {
        if (timer > attackDuration) {
            state = CombatManagerState.Idle;
            return;
        }

        timer += Time.deltaTime;
    }
}
