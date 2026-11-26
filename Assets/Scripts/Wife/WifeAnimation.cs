using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WifeAnimation : MonoBehaviour {
    //ANIMATION PARAMS NAMES
    private const string RUNNING_PARAM_NAME         = "IsRunning";
    private const string SPRINTING_PARAM_NAME       = "IsSprinting";
    private const string LYING_DOWN_TRIGGER_NAME    = "TrgLieDown";
    private const string STAND_UP_TRIGGER_NAME      = "TrgStandUp";
    
    //COMPONENTS
    WifeStateMachine _stateMachine;
    Animator _animator;

    //METHODS
    private void OnEnable () {
        WifeController.OnEnableMovement     += HandleOnEnableMovement;
        WifeController.OnDisableMovement    += HandleOnDisableMovement;
    }
    private void OnDisable () {
        WifeController.OnEnableMovement     -= HandleOnEnableMovement;
        WifeController.OnDisableMovement    -= HandleOnDisableMovement;
    }
    void Start () {
        _stateMachine   = GetComponentInParent<WifeStateMachine>();
        _animator       = GetComponent<Animator>();
    }
    void Update() {
        switch (_stateMachine.CurrentState) {
            case WifeMovementState.Idle:
                PlayIdleAnimation();
                break;
            case WifeMovementState.Running: 
                PlayRunningAnimation();
                break;
            case WifeMovementState.Sprinting: 
                PlaySprintingAnimation();
                break;
            default: 
                break;
        }
    }

    private void PlayIdleAnimation () { 
        _animator.SetBool(SPRINTING_PARAM_NAME, false);
        _animator.SetBool(RUNNING_PARAM_NAME, false);
    }
    private void PlayRunningAnimation () { 
        _animator.SetBool(SPRINTING_PARAM_NAME, false);
        _animator.SetBool(RUNNING_PARAM_NAME, true);
    }
    private void PlaySprintingAnimation() { 
        _animator.SetBool(SPRINTING_PARAM_NAME, true);
        _animator.SetBool(RUNNING_PARAM_NAME, false);
    }
    public void PlayLyingDownAnimation () {
        if (_stateMachine.CurrentState == WifeMovementState.LyingDown ||
            _stateMachine.CurrentState == WifeMovementState.StandingUp) 
            return;
        Debug.Log("WifeAnimation: PlayLyingAnimation() - State: " + _stateMachine.CurrentState);
        PlayIdleAnimation();
        _animator.SetTrigger(LYING_DOWN_TRIGGER_NAME);
    }
    public void PlayStandingUpAnimation () {
        if (_stateMachine.CurrentState == WifeMovementState.StandingUp || 
            _stateMachine.CurrentState == WifeMovementState.LyingDown) 
            return;

        PlayIdleAnimation();
        _animator.SetTrigger(STAND_UP_TRIGGER_NAME);
    }
    private void HandleOnDisableMovement () {
        PlayLyingDownAnimation();
        _stateMachine.CurrentState = WifeMovementState.LyingDown;
    }
    private void HandleOnEnableMovement () {
        PlayStandingUpAnimation();
        _stateMachine.CurrentState = WifeMovementState.StandingUp;
    }
}
