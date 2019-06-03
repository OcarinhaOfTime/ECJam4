using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    [System.Serializable]
    public class GameData {
        public CharacterDataObject status;
        public Dictionary<int, int> itemsDict;

        public GameData(CharacterDataObject status, Dictionary<int, int> itemsDict) {
            this.status = status;
            this.itemsDict = itemsDict;
        }
    }

    public static DataManager instance;
    public CharacterData characterData;
    public InventoryManager inventoryManager;

    public const string path = "daiiko_mahou.dat";

    private void Awake() {
        instance = this;
    }

    [ContextMenu("Save")]
    public void Save() {
        var gameData = new GameData(new CharacterDataObject(characterData), inventoryManager.itemsDict);
        FileUtils.Save(path, gameData);
        Debug.Log("Game saved");
    }

    public bool SaveExists() {
        return FileUtils.FileExists(path);
    }

    [ContextMenu("Load")]
    public void Load() {
        GameData gameData = FileUtils.Load<GameData>(path);
        inventoryManager.itemsDict = gameData.itemsDict;
        gameData.status.UpdateCharacterData(characterData);
        Debug.Log("Game loaded");
    }

    public void ResetAll() {
        if (SaveExists()) {
            FileUtils.Delete(path);
        }

        characterData.ResetStatus();
    }
}
