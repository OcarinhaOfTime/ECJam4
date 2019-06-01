using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour {
    public int max_hp = 10;
    public int hp = 10;
    public int atk = 5;

    public UnityEvent onTakeDamage = new UnityEvent();
    public UnityEvent onDie = new UnityEvent();
    public BoxCollider2D boxCollider;

    private void Awake() {
        hp = max_hp;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(int damage) {
        hp = Mathf.Max(0, hp - damage);
        onTakeDamage.Invoke();
        if (hp == 0)
            onDie.Invoke();
    }
}
