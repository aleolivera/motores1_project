using UnityEngine;

public class WifeAnimationEvents : MonoBehaviour{
    WifeController _controller;
    WifeStateMachine _stateMachine;
    WifeAnimation _animator;

    private void Start () {
        _controller     = GetComponentInParent<WifeController>();
        _stateMachine   = GetComponentInParent<WifeStateMachine>();
        _animator       = GetComponent<WifeAnimation>();
    }

    public void EnableMovement () { 
        _controller.IsActive        = true;
        _stateMachine.CurrentState  = WifeMovementState.Idle;
    }
    public void DisableMovement () => _stateMachine.CurrentState = WifeMovementState.LaidDown;
    
}
