using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character {
    private void Start() {
        GetComponent<EventTrigger>().onTriggerEnter.AddListener(OnTriggerEnter);
    }

    private void OnTriggerEnter() {
        print("battle time");
        GameManager.instance.StartBossCombat(this);
    }
}
