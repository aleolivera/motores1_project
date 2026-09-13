using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PatrolLocation : MonoBehaviour{
    [Header("Location Data")] 
    [SerializeField] private float _patrolTime;

    BoxCollider _collider;

    public float PatrolTime { 
        get         { return _patrolTime; }
        private set { _patrolTime = value; } 
    }
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }
}
