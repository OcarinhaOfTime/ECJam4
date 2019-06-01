using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private void Start() {
        GetComponent<EventTrigger>().onTriggerEnter.AddListener(OnTriggerEnter);
    }

    private void OnTriggerEnter() {
        print("battle time");
        GameManager.instance.StartCombat(this);
    }
}
