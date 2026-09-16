using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {
    [Header("Quest Data")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private List<KeyItem> _keyItems;
    [SerializeField] private bool _questCompleted;
    public bool IsQuestCompleted { 
        get         { return _questCompleted;  } 
        private set { _questCompleted = value; } 
    }

    public List<KeyItem> Items {
        get { return _keyItems; } 
    }

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerEnter(Collider collider) {
        if(!IsQuestCompleted && collider.gameObject.tag.Equals("Player")) {
            PlayerInventory inventory = GetComponent<PlayerInventory>();
            if(inventory == null) {
                Debug.LogWarning("QuestGiver: Inventory in null");
                return;
            }

            int count = 0;
            foreach(KeyItem item in _keyItems) {
                if(inventory.HasItem(item.Id)) {
                    count++;
                }
            }

            if(count == _keyItems.Count) {
                IsQuestCompleted = true;
                foreach(KeyItem item in _keyItems) {
                    inventory.RemoveItem(item.Id);
                }
            }
        }
    }


}
