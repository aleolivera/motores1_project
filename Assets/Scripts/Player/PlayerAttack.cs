using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputListener))]
[RequireComponent(typeof(GroundDetection))]
public class PlayerAttack : MonoBehaviour {
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _cooldownCounter;
    GroundDetection _groundCheck;
    PlayerInputListener _input;
    PlayerMeleeWeapon _meleeWeapon;

    public bool CanAttack {
        get {
            return ( _cooldownCounter <= 0f && _groundCheck.IsGrounded );
        }
    }

    private void Awake() {
        _cooldownCounter = 0f;
    }

    void Start() {
        _groundCheck    = GetComponent<GroundDetection>();
        _input          = GetComponent<PlayerInputListener>();
        _meleeWeapon    = GetComponentInChildren<PlayerMeleeWeapon>();
        
        if(_groundCheck == null) 
            Debug.LogWarning("PlayerAttack: Ground check not found");
        
        if(_input == null) 
            Debug.LogWarning("PlayerAttack: Player Input Listener not found");
        
        if(_meleeWeapon == null) 
            Debug.LogWarning("PlayerAttack: Melee weapon not found");
    }

    void Update() {
        AttackCooldown();
        HandleAttack();
    }

    public void HandleAttack () {
        if (_input.Attack && CanAttack) {
            PlayerStateMachine.Instance.CurrentState = PlayerState.Attacking;
            _cooldownCounter = _attackCooldown;
        }
    }

    private void AttackCooldown() {
        if (_cooldownCounter > 0f) {
            _cooldownCounter -= Time.deltaTime;
        }
    }

    public void DealDamage (EnemyHealth enemy, int damage) {
        if (enemy == null) return;

        enemy.DealDamage(damage);
    }

    public void EnableMeleeWeaponCollider(bool enabled) {
        _meleeWeapon.EnableCollider(enabled);
    }
}
