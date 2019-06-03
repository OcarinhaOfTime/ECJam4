using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    private void Start() {
        GetComponent<EventTrigger>().onTriggerEnter.AddListener(OnTriggerEnter);
        var sr = GetComponent<SpriteRenderer>();
        sr.color = data.color;
        sr.sprite = data.sprite;
    }

    private void OnTriggerEnter() {
        print("battle time");
        GameManager.instance.StartCombat(this);
    }
}
