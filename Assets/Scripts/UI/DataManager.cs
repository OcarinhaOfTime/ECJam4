using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    [System.Serializable]
    public class GameData {
        public CharacterDataObject status;
        public Dictionary<string, int> itemsDict;
    }
    public CharacterData characterData;
    public InventoryManager inventoryManager;

    public void UpdateCharacterData(CharacterData enemy) {

    }

    public void UpdateItems(Dictionary<string, int> itemsDict) {

    }

    public void Save() {

    }

    public void Load() {

    }
}
