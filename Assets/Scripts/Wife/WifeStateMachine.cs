using UnityEngine;

public enum WifeMovementState { LyingDown, StandingUp, Idle, Running, Sprinting, LaidDown }
public class WifeStateMachine : MonoBehaviour {
    [Header("State Debug")]
    [SerializeField] private WifeMovementState _defaultState = WifeMovementState.LaidDown;
    [SerializeField] private WifeMovementState _state;

    public WifeMovementState DefaultState { 
        get         { return _defaultState; } 
        private set { _state = value; } 
    }

    public WifeMovementState CurrentState { 
        get { return _state; }  
        set { _state = value; } 
    }

    private void Awake () => CurrentState = DefaultState;
}
