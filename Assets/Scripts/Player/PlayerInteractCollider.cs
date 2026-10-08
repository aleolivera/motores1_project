using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerInteractCollider : MonoBehaviour {
    public static event Action<QuestItem> OnQuestItemCollected;
    public static event Action<QuestGiver> OnQuestItemRetrieve;
    public static event Action<KeyItem> OnKeyItemCollected;
    public static event Action<Lock> OnKeyItemRetrieve;

    BoxCollider _collider;
    
    public bool ColliderEnable { 
        get { return _collider.enabled; } 
        set { _collider.enabled = value; } 
    }

    void Start () {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _collider.isTrigger = true;
    }
    
    private void HandleTriggerKeyItem(KeyItem item) {
        if (item == null) {
            Debug.LogWarning("PlayerInteractCollider: KeyItem is null");
            return;
        }

        OnKeyItemCollected?.Invoke(item);
        item.gameObject.SetActive(false);
    }

    private void HandleTriggerQuestItem (QuestItem item) {
        if (item == null) {
            Debug.LogWarning("PlayerInteractCollider: QuestItem is null");
            return;
        }

        OnQuestItemCollected?.Invoke(item);
        item.gameObject.SetActive(false);
    }
    private void HandleTriggerQuestGiver (QuestGiver quest) {
        if (quest == null) {
            Debug.LogWarning("PlayerInteractCollider: Quest Giver is null");
            return;
        }

        OnQuestItemRetrieve?.Invoke(quest);
    }
    private void HandleTriggerLock(Lock loc) {
        if (loc == null) {
            Debug.LogWarning("PlayerInteractCollider: Lock is null");
            return;
        }

        OnKeyItemRetrieve?.Invoke(loc);
    }

    private void OnTriggerEnter (Collider collider) {
        if (collider.gameObject.CompareTag("KeyItem")) {
            _collider.enabled = false;
            HandleTriggerKeyItem(collider.GetComponent<KeyItem>());
        }

        if (collider.gameObject.CompareTag("QuestItem")) {
            _collider.enabled = false;
            HandleTriggerQuestItem(collider.GetComponent<QuestItem>());
        }

        if (collider.gameObject.CompareTag("QuestGiver")) {
            _collider.enabled = false;
            HandleTriggerQuestGiver(collider.GetComponent<QuestGiver>());
        }

        if (collider.gameObject.CompareTag("Lock")) {
            _collider.enabled = false;
            HandleTriggerLock(collider.GetComponent<Lock>());
        }

    }
}
