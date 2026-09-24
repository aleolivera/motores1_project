using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    const string WALKING = "IsWalking";
    const string RUNNING = "IsRunning";
    const string ATTACK = "Attack";
    const string UNCONSCIOUS = "IsUnconscious";

    Animator _animator;
    EnemyIA _enemy;

    void Start() {
        _animator   = GetComponent<Animator>();
        _enemy      = GetComponentInParent<EnemyIA>();

        if(_animator == null)
            Debug.LogError("EnemyAnimation: Animator not found.");
        if(_enemy == null)
            Debug.LogError("EnemyAnimation: EnemyIA not found.");
    }

    void Update() {
        
        switch(_enemy.MovementState) {
            case MovementState.Idle:        PlayIdle();         break;
            case MovementState.Walking:     PlayWalking();      break;
            case MovementState.Running:     PlayRunning();      break;
            case MovementState.Attacking:   PlayAttack();       break;
            case MovementState.Unconscious: PlayUnconscious();  break;
            default: break;
        }
        
    }

    public void PlayIdle() {
        _animator.SetBool(WALKING, false);
        _animator.SetBool(RUNNING, false);
        _animator.SetBool(UNCONSCIOUS, false);
    }

    public void PlayWalking() {
        if(_animator.GetBool(WALKING))  return;
        
        _animator.SetBool(WALKING, true);
        _animator.SetBool(RUNNING, false);
    }

    public void PlayRunning() {
        if(_animator.GetBool(RUNNING))  return;

        _animator.SetBool(RUNNING, true);
        _animator.SetBool(WALKING, false);
    }

    public void PlayAttack() {
        _animator.SetTrigger(ATTACK);
    }

    public void PlayUnconscious() {
        _animator.SetBool(UNCONSCIOUS, true);
    }
}
