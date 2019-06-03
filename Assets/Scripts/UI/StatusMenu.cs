using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusMenu : MonoBehaviour {
    public StatusItem statusItemPrefab;
    public RectTransform statusItemsContent;
    public TMP_Text currentXP;
    public TMP_Text nextXP;
    public TMP_Text level;
    public TMP_Text description;

    public Character player;
    public CharacterDataDescription playerDataDesc;

    private List<StatusItem> items = new List<StatusItem>();
    private CharacterDataObject cdo;

    private void Start() {
        GetComponent<CanvasUI>().onAppear.AddListener(OnAppear);
    }

    private void Clear() {
        foreach (var i in items)
            Destroy(i.gameObject);

        items.Clear();
    }


    private void OnAppear() {
        cdo = new CharacterDataObject(player.data);
        Clear();

        foreach (var o in cdo.UpdatableValues()) {
            var statusItem = Instantiate(statusItemPrefab);
            statusItem.transform.SetParent(statusItemsContent, false);
            statusItem.UpdateValues(o.Key, o.Value);
            statusItem.gameObject.SetActive(true);
            items.Add(statusItem);
        }

        level.text = "Level " + player.data.level;
        currentXP.text = "Mahou " + player.data.xp;
        nextXP.text = "Mahou to next level " + player.data.nextXP;
    }

    public void OnStatusClick(int dir, string id) {
        print("stats " + id + " " + dir);
        if(player.data.xp >= player.data.nextXP) {
            cdo.IncrementValue(id);
            cdo.UpdateCharacterData(player.data);
            player.data.xp -= player.data.nextXP;
            player.data.level++;
            if(id == "vitality") {
                player.Heal(5);
            }
            OnAppear();
        } else {
            print("not enough mahou");
        }
    }

    public void OnStatusHover(string id) {
        description.text = playerDataDesc.GetByID(id);
    }
}
