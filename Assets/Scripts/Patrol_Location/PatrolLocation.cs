using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PatrolLocation : MonoBehaviour{
    [Header("Location Data")] 
    [SerializeField] private float _patrolTime;

    [Header("Gizmo Settings")]
    [SerializeField] private float _cubeSizes;

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

    private void OnDrawGizmos () {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(
            transform.position + (Vector3.up * 0.5f), 
            new Vector3(_cubeSizes, _cubeSizes, _cubeSizes));
    }

}
