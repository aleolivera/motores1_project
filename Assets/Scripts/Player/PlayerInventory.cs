using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour {
    [Header("Inventory")]
    private Dictionary<int, KeyItem> _inventory = new Dictionary<int, KeyItem>();
    public Dictionary<int, KeyItem> Inventory { 
        get { return _inventory; } 
    }

    void Start() { }

    // Update is called once per frame
    void Update() { }

    public bool HasItem(int key) {
        
        return _inventory.ContainsKey(key);
    }
    public void RemoveItem(int key) {
        Debug.Log("Remove item: " + key + " - " + _inventory[key].Name);
        _inventory.Remove(key);
    }

    public void AddItem(KeyItem item) {
        Debug.Log("Add item: " + item.Id + " - " + item.Name);
        _inventory.Add(item.Id, item);
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag.Equals("KeyItem")) {
            KeyItem item = collider.GetComponent<KeyItem>();
            if(item == null) {
                Debug.LogWarning("PlayerInventory: KeyItem is null");
                return;
            }

            AddItem(item);
            item.gameObject.SetActive(false);
        }
    }
}
