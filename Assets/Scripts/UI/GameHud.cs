using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHud : MonoBehaviour {
    public TMP_Text playerHP;
    public Character character;

    private void Start() {
        character.onChange.AddListener(UpdateHP);
        UpdateHP();
    }

    private void UpdateHP() {
        playerHP.text = character.hp + "/" + character.max_hp;
    }
}
