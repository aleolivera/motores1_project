using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    const string IS_WALKING = "IsWalking";
    const string IS_RUNNING = "IsRunning";
    const string TRG_ATTACK = "Attack";
    const string TRG_UNCONSCIOUS = "Unconscious";
    const string TRG_WAKE_UP = "WakeUp";

    Animator _animator;
    EnemyAttack _attack;
    EnemyIA _enemy;

    private void OnEnable () {
        EnemyStateMachine.OnStateChangeToIdle       += PlayIdle;
        EnemyStateMachine.OnStateChangeToWalking    += PlayWalking;
        EnemyStateMachine.OnStateChangeToRunning    += PlayRunning;
        EnemyStateMachine.OnStateChangeToAttacking  += PlayAttack;
        EnemyStateMachine.OnStateChangeToUnconcious += PlayUnconscious;
        EnemyStateMachine.OnStateChangeToWakeUp     += PlayWakeUp;
    }
    private void OnDisable () {
        EnemyStateMachine.OnStateChangeToIdle       -= PlayIdle;
        EnemyStateMachine.OnStateChangeToWalking    -= PlayWalking;
        EnemyStateMachine.OnStateChangeToRunning    -= PlayRunning;
        EnemyStateMachine.OnStateChangeToAttacking  -= PlayAttack;
        EnemyStateMachine.OnStateChangeToUnconcious -= PlayUnconscious;
        EnemyStateMachine.OnStateChangeToWakeUp     -= PlayWakeUp;
    }

    void Start() {
        _animator   = GetComponent<Animator>();
        _attack     = GetComponentInParent<EnemyAttack>();
        _enemy      = GetComponentInParent<EnemyIA>();

        if(_animator == null)
            Debug.LogError("EnemyAnimation: Animator not found.");
        if(_enemy == null)
            Debug.LogError("EnemyAnimation: Enemy IA not found.");
    }

    public void PlayIdle() {
        _animator.SetBool(IS_WALKING, false);
        _animator.SetBool(IS_RUNNING, false);
    }

    public void PlayWalking() {
        if(_animator.GetBool(IS_WALKING))  return;
        
        _animator.SetBool(IS_WALKING, true);
        _animator.SetBool(IS_RUNNING, false);
    }

    public void PlayRunning() {
        if(_animator.GetBool(IS_RUNNING))  return;

        _animator.SetBool(IS_RUNNING, true);
        _animator.SetBool(IS_WALKING, false);
    }

    public void PlayAttack() {
        _animator.SetTrigger(TRG_ATTACK);

        _animator.SetBool(IS_WALKING, false);
        _animator.SetBool(IS_RUNNING, false);
    }

    public void PlayUnconscious() {
        _animator.SetTrigger(TRG_UNCONSCIOUS);

        _animator.SetBool(IS_WALKING, false);
        _animator.SetBool(IS_RUNNING, false);
    }
    public void PlayWakeUp() {
        _animator.SetTrigger(TRG_WAKE_UP);

        _animator.SetBool(IS_WALKING, false);
        _animator.SetBool(IS_RUNNING, false);
    }

    public void Shoot () {
        _attack.HandleAttack();
    }

    public void BackToPatrol () {
        _enemy.BackToPatrol();
    }

    public void OnShotAnimationEnd () {
        EnemyStateMachine.State = EnemyActionState.Idle;
    }

}
