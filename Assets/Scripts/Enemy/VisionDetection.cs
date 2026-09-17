using System.Collections.Generic;
using UnityEngine;

public class VisionDetection : MonoBehaviour {
    [Header("Vision")]
    [SerializeField] private float _checkDistance = 16f;
    [SerializeField] private float _alertDistance = 8f;
    [SerializeField] private LayerMask _detectionLayers;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private List<Transform> _visionLines = new List<Transform>(9);

    private Color _gizmoColor = Color.green;

    public float AlertDistance { get { return _alertDistance; } }
    public Color GizmoColor { 
        get { return _gizmoColor; } 
        set { _gizmoColor = value; } 
    }

    void Start() {
        Gizmos.color = Color.green;
        if(_visionLines.Count == 0)
            Debug.Log("EnemyMovement: Vision lines is empty.");
    }
    public Vector3 CheckVision() {
        bool hasHit = false;
        RaycastHit hit;
        foreach(Transform line in _visionLines) {
            hasHit = Physics.Raycast(
                                    line.position,
                                    line.forward,
                                    out hit,
                                    _checkDistance,
                                    _detectionLayers);
            if(hasHit) {
                int playerLayerHit = (_playerLayer.value & (1 << hit.collider.gameObject.layer));
                if(playerLayerHit != 0)
                    return hit.transform.position;
            }
        }
        return Vector3.zero;
    }

    private void OnDrawGizmos() {
        Gizmos.color = _gizmoColor;

        foreach(Transform line in _visionLines) {
            Gizmos.DrawLine(
                        line.position,
                        line.position + (line.forward * _checkDistance));
        }
    }
}
