using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemsMenu : MonoBehaviour {
    public UsableItem prefab;
    private InventoryManager inventoryManager;
    public TMP_Text description;
    public RectTransform content;
    public RectTransform empty;
    public Character character;

    private List<UsableItem> items = new List<UsableItem>();
    public UnityEvent onItemUsed;

    private void Start() {
        inventoryManager = DataManager.instance.inventoryManager;
        GetComponent<CanvasUI>().onAppear.AddListener(OnAppear);
    }

    private void Clear() {
        foreach (var i in items)
            Destroy(i.gameObject);

        items.Clear();
    }

    private void OnAppear() {
        Clear();
        foreach (var o in inventoryManager.itemsDict) {
            if (o.Value <= 0)
                continue;
            var item = Instantiate(prefab);
            item.transform.SetParent(content, false);
            item.UpdateValues(o.Key, inventoryManager.items[o.Key], inventoryManager.itemsDict[o.Key]);
            item.gameObject.SetActive(true);
            items.Add(item);
        }

        var emp = items.Count <= 0;
        empty.gameObject.SetActive(emp);
        content.gameObject.SetActive(!emp);
    }


    public void UseItem(int itemID) {
        PopupCanvas.instance.ShowOptionPopup("Use " + inventoryManager.items[itemID] + "?", () => {
            inventoryManager.UseItem(character, itemID);
            OnAppear();
            onItemUsed.Invoke();
        }, () => { });
    }

    public void HoverItem(int itemID) {
        description.text = inventoryManager.itemsDesc[itemID];
    }
}
