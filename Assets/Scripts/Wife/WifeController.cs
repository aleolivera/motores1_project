using System;
using UnityEngine;

public class WifeController : MonoBehaviour {
    //INSPECTOR
    [Header("Wife Settings")]
    [SerializeField] private Quest _questRef;
    [SerializeField] private float _stopDistance = 0.5f;
    [SerializeField] private float _sprintDistance = 3f;
    [SerializeField] private float _disableDistance = 6f;

    [Header("Debug")]
    [SerializeField] GameObject _playerRef;
    [SerializeField] bool _isActive;
    
    // COMPONENTS
    WifeMovement _movement;
    WifeStateMachine _state;
    
    // PROPERTIES
    public bool IsActive { 
        get { return _isActive; } 
        set { _isActive = value; } 
    }
    public Quest QuestRef { 
        get         { return _questRef; }  
        private set { _questRef = value; } 
    }
    public GameObject PlayerRef{ 
        get { return _playerRef; }  
        set { _playerRef = value; } 
    }

    //EVENTS
    public static event Action OnEnableMovement;
    public static event Action OnDisableMovement;

    //METHODS
    private void Awake () => IsActive = (QuestRef == null) ? true : false;

    private void OnEnable () => 
        WifeCollision.OnCollisionWithPlayer += HandleOnCollisionWithPlayer;
    
    private void OnDisable () => 
        WifeCollision.OnCollisionWithPlayer -= HandleOnCollisionWithPlayer;
    
    void Start () {
        _movement   = GetComponent<WifeMovement>();
        _state      = GetComponent<WifeStateMachine>();
    }
    private void Update () {
        if (!IsActive || _playerRef == null) return;

        float distanceToPlayer = Vector3.Distance(
                                            transform.position, 
                                            _playerRef.gameObject.transform.position);
        
        if (distanceToPlayer > _disableDistance) {
            _state.CurrentState = WifeMovementState.Idle;
            _movement.Stop();
            DisableMovement();
            return;
        }

        if (distanceToPlayer > _sprintDistance) {
            _state.CurrentState = WifeMovementState.Sprinting;
            _movement.Sprint(_playerRef.transform.position);
            return;
        }

        if (distanceToPlayer > _stopDistance) {
            _state.CurrentState = WifeMovementState.Running;
            _movement.Run(_playerRef.transform.position);
            return;
        }

        _state.CurrentState = WifeMovementState.Idle;
        _movement.Stop();
    }
    public void DisableMovement () {
        Debug.Log("WifeController: Disable Movement()");
        IsActive = false;
        _playerRef = null;
        OnDisableMovement?.Invoke();
    }
    private void HandleOnCollisionWithPlayer (GameObject player) {
        if (player == null || !(_state.CurrentState == WifeMovementState.LaidDown)) return;
        
        if (QuestRef == null || QuestRef.IsQuestCompleted) {
            _playerRef  = player;
            OnEnableMovement?.Invoke();
        }
    }
}
