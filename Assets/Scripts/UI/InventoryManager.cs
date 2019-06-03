using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager instance;
    public Dictionary<int, int> itemsDict = new Dictionary<int, int>();

    public static string[] items = { "Small Potion", "Potion", "Large Potion", "Elixir" };
    public static string[] itemsDesc = {
        "Heals 10 HP.",
        "Heals 25 HP.",
        "Heals 50 HP.",
        "Heals all HP."
    };

    private void Awake() {
        instance = this;
    }

    public void UseItem(Character character, int itemID) {
        var itemKey = itemID;
        if (!itemsDict.ContainsKey(itemKey)) {
            Debug.LogError(itemKey + " doesn't exist");
            return;
        }
        if(itemsDict[itemKey] <= 0) {
            Debug.LogError(itemKey + " empty");
            return;
        }

        switch (itemID) {
            case 0:
                character.Heal(10);
                break;
            case 1:
                character.Heal(25);
                break;
            case 2:
                character.Heal(50);
                break;
            case 3:
                character.Heal(character.max_hp);
                break;
        }
        itemsDict[itemKey]--;
    }

    public void AddItem(int itemID) {
        var itemKey = itemID;
        if (itemsDict.ContainsKey(itemKey)) {
            itemsDict[itemKey]++;
            print("incremented " + itemID);
        } else {
            itemsDict.Add(itemKey, 1);
            print("added " + itemID);
        }
    }

    public int randomItem {
        get {
            return Random.Range(0, items.Length);
        }
    }

    [ContextMenu("Test")]
    public void RandomTest() {
        for (int i = 0; i < 5; i++) {
            var r = randomItem;
            AddItem(r);
            print("added " + items[r]);
        }        
    }
}
