﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour {
    public CharacterData data;
    public int hp = 10;
    public int max_hp {
        get {
            return data.vitality * 5;
        }
    }

    public UnityEvent onChange;

    protected virtual void Awake() {
        hp = max_hp;
    }

    public void TakeDamage(int damage) {
        hp = Mathf.Max(0, hp - damage);
        onChange.Invoke();
    }

    public void Heal(int ammount) {
        hp = Mathf.Min(max_hp, hp + ammount);
        onChange.Invoke();
    }
}
