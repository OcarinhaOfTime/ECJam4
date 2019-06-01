using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour {
    public CharacterData data;
    public int hp = 10;

    protected virtual void Awake() {
        hp = data.vitality;
    }

    public void TakeDamage(int damage) {
        hp = Mathf.Max(0, hp - damage);
    }
}
