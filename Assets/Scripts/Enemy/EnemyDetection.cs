using UnityEngine;

public enum DetectionState { OnPatrol, OnCaution, OnAlert, Checking, Disabled }
public class EnemyDetection : MonoBehaviour {
    [Header("Detection State")]
    [SerializeField] private DetectionState _state = DetectionState.OnPatrol;
    [Header("Time")]
    [SerializeField] private float _patrolTime = 4f;
    [SerializeField] private float _cautionTime = 6f;
    [SerializeField] private float _checkTime = 6f;
    [SerializeField] private float _alertTime = 6f;
    [SerializeField] private float _disableTime = 20f;
    [SerializeField] private float _visionTime = 0.2f;

    public DetectionState State { 
        get { return _state; } set { _state = value; } 
    }
    public float PatrolTime { 
        get { return _patrolTime; } set { _patrolTime = value; } 
    }
    public float CautionTime { 
        get { return _cautionTime; } set { _cautionTime = value; } 
    }
    public float CheckTime { 
        get { return _checkTime; } set { _checkTime = value; } 
    }
    public float AlertTime { 
        get { return _alertTime; } set { _alertTime= value; } 
    }
    public float DisableTime {
        get { return _disableTime; } set { _disableTime = value; } 
    }
    public float VisionTime { 
        get { return _visionTime; } set { _visionTime = value; } 
    }
}
