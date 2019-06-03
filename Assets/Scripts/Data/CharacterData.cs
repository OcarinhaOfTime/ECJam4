using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Character Data")]
public class CharacterData : ScriptableObject {
    public string characterName = "";
    public int vitality = 5;
    public int strength = 3;
    public int intelligence = 3;
    public int resistance = 3;
    public int agility = 3;
    public int skill = 3;
    public int luck = 3;
    public int gold = 0;
    public int xp = 0;
    public int level = 0;

    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public Color color = Color.white;

    [ContextMenu("ResetStatus")]
    public void ResetStatus() {
        vitality = 5;
        strength = 3;
        intelligence = 3;
        resistance = 3;
        agility = 3;
        skill = 3;
        luck = 3;
        gold = 0;
        xp = 0;
        level = 0;
}

    public int nextXP {
        get {
            return 100 + level * 10;
        }
    }
}
