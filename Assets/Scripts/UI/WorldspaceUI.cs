using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SphereCollider))]
public class WorldspaceUI : MonoBehaviour {
    [Header("World space UI")]
    [SerializeField] List<GameObject> _UIElements;
    [SerializeField] float visibilityDistance = 5f;
    SphereCollider _collider;

    private void Start () {
        _collider = GetComponent<SphereCollider>();
        if(_collider == null) {
            Debug.LogWarning("WorldspaceUI: Collider is null");
        }

        _collider.enabled = true;
        _collider.isTrigger = true;
        _collider.radius = visibilityDistance;

        EnableUIElements(false);
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            EnableUIElements(true);
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            EnableUIElements(false);
        }
    }

    private void EnableUIElements(bool enabled) {
        if (_UIElements.Count < 1) return;

        foreach (GameObject element in _UIElements) {
            element.SetActive(enabled);
        }
    }
}
