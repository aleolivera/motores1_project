using Mono.Cecil.Cil;
using System;
using UnityEngine;

public enum EnemyActionState { Idle, Walking, Running, Attacking, Unconscious, WakeUp }
public class EnemyStateMachine : MonoBehaviour {
    [Header("Enemy State Debug")]
    [SerializeField] private EnemyActionState _state = EnemyActionState.Idle;
    public event Action OnStateChangeToIdle;
    public event Action OnStateChangeToWalking;
    public event Action OnStateChangeToRunning;
    public event Action OnStateChangeToAttacking;
    public event Action OnStateChangeToUnconcious;
    public event Action OnStateChangeToWakeUp;

    public EnemyActionState State {  
        get { return _state; }
        set {  
            _state = value;
            switch (_state) {
                case EnemyActionState.Idle:         OnStateChangeToIdle?.Invoke();          break;
                case EnemyActionState.Walking:      OnStateChangeToWalking?.Invoke();       break;
                case EnemyActionState.Running:      OnStateChangeToRunning?.Invoke();       break;
                case EnemyActionState.Attacking:    OnStateChangeToAttacking?.Invoke();     break;
                case EnemyActionState.Unconscious:  OnStateChangeToUnconcious?.Invoke();    break;
                case EnemyActionState.WakeUp:       OnStateChangeToWakeUp?.Invoke();        break;
                default: break;
            }
        }
    }
}
