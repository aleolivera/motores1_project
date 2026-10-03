using System;
using UnityEngine;

public enum PlayerState { Idle, Sneaking, Sprinting, Attacking, Interacting, Jumping, Dead }
public class PlayerStateMachine : MonoBehaviour {
    [Header("Player State Machine")]
    [SerializeField] private PlayerState _state = PlayerState.Idle;
    public static event Action OnStateChangedToIdle;
    public static event Action OnStateChangedToSneaking;
    public static event Action OnStateChangedToSprinting;
    public static event Action OnStateChangedToInteracting;
    public static event Action OnStateChangedToAttack;
    public static event Action OnStateChangedToDead;

    private static PlayerStateMachine _instance;
    public static PlayerStateMachine Instance { get { return _instance; } }

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        } else {
            Destroy(_instance);
        }
    }

    public PlayerState CurrentState {
        get { return _state; }
        set { 
            _state = value;
            switch (_state) { 
                case PlayerState.Idle:          OnStateChangedToIdle?.Invoke();         break;
                case PlayerState.Sneaking:      OnStateChangedToSneaking?.Invoke();     break;
                case PlayerState.Sprinting:     OnStateChangedToSprinting?.Invoke();    break;
                case PlayerState.Interacting:   OnStateChangedToInteracting?.Invoke();  break;
                case PlayerState.Attacking:     OnStateChangedToAttack?.Invoke();       break;
                case PlayerState.Dead:          OnStateChangedToDead?.Invoke();         break;
                default: break;
            }
        }
    }
}
