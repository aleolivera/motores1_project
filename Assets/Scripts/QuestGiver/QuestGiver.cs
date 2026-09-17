using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {
    [Header("Quest Data")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private List<KeyItem> _keyItems;
    [SerializeField] private bool _questCompleted = false;
    
    public bool IsQuestCompleted { 
        get         { return _questCompleted;  } 
        private set { _questCompleted = value; } 
    }
    public List<KeyItem> Items { 
        get { return _keyItems; } 
    }
    public int Id {
        get { return _id; } 
    }

    private void OnTriggerStay(Collider collider) {
        if(!IsQuestCompleted && collider.gameObject.tag.Equals("Player")) {
            PlayerInventory inventory = collider.GetComponent<PlayerInventory>();

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
                LevelManager.Instance.QuestCompleted(Id);
            }
        }
    }


}
