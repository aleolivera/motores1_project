using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    [Header("Inventory")]
    private Dictionary<int, QuestItem> _questItems = new Dictionary<int, QuestItem>();
    private Dictionary<int, KeyItem> _keyItems = new Dictionary<int, KeyItem>();

    public Dictionary<int,QuestItem> QuestItems { 
        get { return _questItems; }  
    }
    public Dictionary<int,KeyItem> KeyItems { 
        get { return _keyItems; }  
    }

    PlayerInteractCollider _interactCollider;

    private void OnEnable () {
        PlayerInteractCollider.OnKeyItemCollected   += AddKeyItem;
        PlayerInteractCollider.OnKeyItemRetrieve    += RetrieveKeyItem;
        PlayerInteractCollider.OnQuestItemCollected += AddQuestItem;
        PlayerInteractCollider.OnQuestItemRetrieve  += RetrieveQuestItem;
    }
    private void OnDisable () {
        PlayerInteractCollider.OnKeyItemCollected   -= AddKeyItem;
        PlayerInteractCollider.OnKeyItemRetrieve    -= RetrieveKeyItem;
        PlayerInteractCollider.OnQuestItemCollected -= AddQuestItem;
        PlayerInteractCollider.OnQuestItemRetrieve  -= RetrieveQuestItem;
    }

    private void Start () {
        _interactCollider = GetComponentInChildren<PlayerInteractCollider>();
        if(_interactCollider == null) {
            Debug.LogError("PlayerInventory: Interact trigger is null.");
        }
    }

    public bool HasQuestItem(int key) {
        return _questItems.ContainsKey(key);
    }
    public void RemoveQuestItem(int key) {
        Debug.Log("Remove quest item: " + key + " - " + _questItems[key].Name);
        _questItems.Remove(key);
    }
    public void AddQuestItem(QuestItem item) {
        if (item == null) return;

        Debug.Log("Add item: " + item.Id + " - " + item.Name);
        _questItems .Add(item.Id, item);
    }

    public void RetrieveQuestItem (QuestGiver quest) {
        if (quest.IsQuestCompleted) return;

        int count = 0;
        foreach (QuestItem item in quest.Items) {
            if (HasQuestItem(item.Id)) {
                count++;
            }
        }
        
        if (count != quest.Items.Count) return;
            
        quest.IsQuestCompleted = true;
        foreach (QuestItem item in quest.Items) {
            RemoveQuestItem(item.Id);
        }

        LevelManager.Instance.QuestCompleted(quest.Id);
    }

    public bool HasKeyItem (int key) {
        return _keyItems.ContainsKey(key);
    }
    public bool RemoveKeyItem (int key) {
        Debug.Log("Remove quest item: " + key + " - " + _keyItems[key].Name);
        return _keyItems.Remove(key);
    }
    public void AddKeyItem (KeyItem item) {
        if (item == null) return;

        Debug.Log("Add quest item: " + item.Id + " - " + item.Name);
        _keyItems.Add(item.Id, item);
    }

    public void RetrieveKeyItem (Lock loc) {
        if (loc.IsOpened) return;

        int count = 0;
        foreach (KeyItem item in loc.Items) {
            if (HasKeyItem(item.Id)) {
                count++;
            }
        }

        if (count != loc.Items.Count) return;

        loc.Open();
        foreach (KeyItem item in loc.Items) {
            RemoveKeyItem(item.Id);
        }
    }

    public void EnableInteractCollider (bool enabled) {
        _interactCollider.ColliderEnable = enabled;
    }
}
