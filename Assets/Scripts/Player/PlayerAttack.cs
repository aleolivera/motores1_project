using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputListener))]
[RequireComponent(typeof(GroundDetection))]
public class PlayerAttack : MonoBehaviour {
    [Header("Attack")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _normalDamage = 10;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _cooldownCounter;

    PlayerInputListener _input;
    GroundDetection _groundCheck;

    private void Awake() {
        _damage = _normalDamage;
        _cooldownCounter = _attackCooldown;
    }

    void Start() { 
        _input = GetComponent<PlayerInputListener>();
        if(_input == null) {
            Debug.LogWarning("PlayerAttack: PlayerInputListener not found");
        }

        _groundCheck = GetComponent<GroundDetection>();
        if(_groundCheck == null) {
            Debug.LogWarning("PlayerAttack: Ground check not found");
        }
    }

    void Update() {
        AttackCooldown();
    }

    public void HandleAttack(EnemyHealth enemy) {
        if(CanAttack()) {
            Debug.Log("Enemy damaged: " + _damage);
            enemy.DealDamage(_damage);
            _cooldownCounter = 0f;
        }
    }

    public void KnockOutEnemy(EnemyHealth enemy) {
        enemy.KnockOut();
        enemy.DealDamage(enemy.MaxHealth);
    }

    private void AttackCooldown() {
        if(_cooldownCounter < _attackCooldown) {
            _cooldownCounter += Time.deltaTime;
        }
    }
    private bool CanAttack() {
        return (_cooldownCounter >= _attackCooldown
                && _groundCheck.IsGrounded);
    }

    private void OnTriggerEnter(Collider collider) {

        if(collider.gameObject.tag.Equals("Enemy")) {
            EnemyHealth enemy = collider.gameObject.GetComponent<EnemyHealth>();
            if(enemy == null) {
                Debug.LogWarning("EnemyHealth is null");
                return;
            }

            if(enemy.Status == EnemyStatus.OnAlert) {
                HandleAttack(enemy);
            } else {
                KnockOutEnemy(enemy);
            }
        }
    }

    private void OnTriggerStay(Collider collider) {
        if(collider.gameObject.tag.Equals("Enemy")) {
            EnemyHealth enemy = collider.gameObject.GetComponent<EnemyHealth>();
            if(enemy == null) {
                Debug.LogWarning("EnemyMovement is null");
                return;
            }

            if(enemy.Status == EnemyStatus.OnAlert) {
                HandleAttack(enemy);
            } 
        }
    }
}
