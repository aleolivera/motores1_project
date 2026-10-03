using Mono.Cecil.Cil;
using System;
using UnityEngine;

public enum EnemyActionState { Idle, Walking, Running, Attacking, Unconscious, WakeUp }
public class EnemyStateMachine : MonoBehaviour {
    [Header("Enemy State Debug")]
    static private EnemyActionState _state = EnemyActionState.Idle;
    [SerializeField] private EnemyActionState _stateDebug;

    public static event Action OnStateChangeToIdle;
    public static event Action OnStateChangeToWalking;
    public static event Action OnStateChangeToRunning;
    public static event Action OnStateChangeToAttacking;
    public static event Action OnStateChangeToUnconcious;
    public static event Action OnStateChangeToWakeUp;

    public static EnemyActionState State {  
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

    private void Update () {
        _stateDebug = _state;
    }

}
