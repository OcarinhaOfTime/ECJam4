﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VUtils;

public class SphereAttackManager : MonoBehaviour {

    public Color[] attackSpheresColors = { Color.red, Color.blue, Color.yellow, Color.red, Color.blue, Color.yellow };
    public Color[] defenceSpheresColors = { Color.gray, Color.gray, Color.gray };

    public SpheresManager attackSpheres;
    public SpheresManager defenceSpheres;
    public EnemyAttack enemyAttack;

    public bool attacking;
    public UnityEvent onAttack = new UnityEvent();

    CharacterHolder player;
    CharacterHolder enemy;

    public bool defenceConnected = false;
    public CollisionUtility.Line defenceLine;
    public CollisionUtility.Line enemyLine;

    public bool attackConnect = false;
    public CollisionUtility.Line attackLine;

    private void Start() {
        attackSpheres.onLineConnect.AddListener(Attack);
        attackSpheres.onConnectFail.AddListener(AttackFail);

        defenceSpheres.onLineConnect.AddListener(Defend);
        defenceSpheres.onConnectFail.AddListener(DefendFail);
    }

    public void Setup(CharacterHolder player, CharacterHolder enemy) {
        this.player = player;
        this.enemy = enemy;

        var acolors = NumEx.Range(player.characterData.intelligence + 3, (int i) => attackSpheresColors[i % attackSpheresColors.Length]);
        var dcolors = NumEx.Range(player.characterData.resistance, (int i) => Color.gray);

        attackSpheres.Create(acolors);
        defenceSpheres.Create(dcolors);
        enemyAttack.Clear();       
    }

    public void ActivateAttack(System.Action onEnd) {
        print("activating attack");
        attackSpheres.SetAllActive(true);
        onAttack.RemoveAllListeners();
        onAttack.AddListener(() => onEnd());
        attackConnect = false;
        attacking = true;
    }

    public void StopAttack() {
        onAttack.RemoveAllListeners();
        attackSpheres.SetAllActive(false);

        attacking = false;
    }

    public void ActivateDefense(System.Action onEnd) {
        defenceConnected = false;
        defenceSpheres.SetAllActive(true);

        enemyAttack.ActivateRandomAttack((l) => OnEnemyAttackEnd(l, onEnd));
    }

    private void OnEnemyAttackEnd(CollisionUtility.Line line, System.Action onEnd) {
        enemyLine = line;
        onEnd();
    }

    public void StopDefense() {
        defenceSpheres.SetAllActive(false);
    }

    private void Attack(CollisionUtility.Line line) {
        attackConnect = CollisionUtility.LineIntersectsRectTest(line, enemy.col);
        attackLine = line;

        onAttack.Invoke();
    }

    private void AttackFail() {
        print("failed");
    }

    private void Defend(CollisionUtility.Line line) {
        defenceConnected = true;
        defenceLine = line;
    }

    private void DefendFail() {
        print("failed");
    }
}
