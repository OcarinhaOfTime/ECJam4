using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour {
    public int hp = 10;

    public UnityEvent onTakeDamage = new UnityEvent();
    public UnityEvent onDie = new UnityEvent();

    public void TakeDamage(int damage) {
        hp = Mathf.Max(0, hp - damage);
        onTakeDamage.Invoke();
        if (hp == 0)
            onDie.Invoke();
    }
}
