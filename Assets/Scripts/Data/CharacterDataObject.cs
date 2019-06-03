using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDataObject {

    public Dictionary<string, int> entries = new Dictionary<string, int>();

    public CharacterDataObject(CharacterData characterData) {
        entries.Add("vitality", characterData.vitality);
        entries.Add("strength", characterData.strength);
        entries.Add("intelligence", characterData.intelligence);
        entries.Add("resistance", characterData.resistance);
        entries.Add("agility", characterData.agility);
        entries.Add("skill", characterData.skill);
        entries.Add("luck", characterData.luck);
        entries.Add("gold", characterData.gold);
        entries.Add("xp", characterData.xp);
        entries.Add("level", characterData.level);
    }

    public void UpdateCharacterData(CharacterData characterData) {
        characterData.vitality = entries["vitality"];
        characterData.strength = entries["strength"];
        characterData.intelligence = entries["intelligence"];
        characterData.resistance = entries["resistance"];
        characterData.agility = entries["agility"];
        characterData.skill = entries["skill"];
        characterData.luck = entries["luck"];
        characterData.gold = entries["gold"];
        characterData.xp = entries["xp"];
        characterData.level = entries["level"];
    }

    public Dictionary<string, int> UpdatableValues() {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        dict.Add("vitality", entries["vitality"]);
        dict.Add("strength", entries["strength"]);
        dict.Add("intelligence", entries["intelligence"]);
        dict.Add("resistance", entries["resistance"]);
        dict.Add("agility", entries["agility"]);
        dict.Add("skill", entries["skill"]);
        dict.Add("luck", entries["luck"]);

        return dict;
    }

    public void IncrementValue(string id, int amm = 1) {
        entries[id] += amm;
    }
}
