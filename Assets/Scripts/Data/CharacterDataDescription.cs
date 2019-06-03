using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/CharacterDataDescription")]
[System.Serializable]
public class CharacterDataDescription : ScriptableObject {
    [System.Serializable]
    public class CharacterDataDescriptionEntry {
        public string entryID;
        [TextArea]public string description;
    }

    public CharacterDataDescriptionEntry[] entries;

    public string GetByID(string id) {
        foreach (var e in entries)
            if (e.entryID == id)
                return e.description;

        return "";
    }
}
