using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Character Data")]
public class CharacterData : ScriptableObject {
    public string characterName = "";
    public int vitality = 10;
    public int strength = 3;
    public int intelligence = 3;
    public int resistance = 3;
    public int agility = 3;
    public int skill = 3;
    public int gold = 10;
    public int xp = 10;
    public int level = 1;

    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public Color color = Color.white;
}
