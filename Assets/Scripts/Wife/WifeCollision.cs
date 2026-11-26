using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class WifeCollision : MonoBehaviour {
    //COMPONENTS
    SphereCollider _collider;
    WifeController _controller;

    //EVENTS
    public static event Action<GameObject> OnCollisionWithPlayer;

    void Start () {
        _controller         = GetComponent<WifeController>();
        _collider           = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Player")) {    
            OnCollisionWithPlayer?.Invoke(other.gameObject);
        }
    }
}
