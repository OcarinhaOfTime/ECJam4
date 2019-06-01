using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VUtils;

public class CombatManager : MonoBehaviour {
    public enum CombatManagerState {
        Idle,
        PlayerAttack,
        EnemyAttack
    }

    public BoxCollider2D enemy;
    public BoxCollider2D player;

    private SphereAttackManager sphereAttackManager;
    public float attackDuration = 5;
    public CombatManagerState state = CombatManagerState.Idle;

    private float timer = 0;

    private void Start() {
        sphereAttackManager = GetComponent<SphereAttackManager>();
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
                sphereAttackManager.ActivateAttack(enemy, OnPlayerAttack);
                break;
            case CombatManagerState.EnemyAttack:
                sphereAttackManager.ActivateAttack(enemy, OnEnemyAttack);
                break;
        }
    }

    private void OnPlayerAttack() {

    }

    private void OnEnemyAttack() {

    }

    private void PlayerAttack() {
        if(timer > attackDuration) {
            state = CombatManagerState.EnemyAttack;
            timer = 0;
            sphereAttackManager.StopAttack();
            return;
        }

        timer += Time.deltaTime;
    }

    private void EnemyAttack() {

    }
}
