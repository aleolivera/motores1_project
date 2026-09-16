using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [Header("Attack")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _normalDamage = 10;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _cooldownCounter;

    EnemyIA _enemyIA;

    private void Awake() {
        /*
        if(_animator == null)
            _animator = GetComponentInParent<Animator>();
        */
        _damage = _normalDamage;
        _cooldownCounter = _attackCooldown;
    }
    void Start() {
        _enemyIA = GetComponent<EnemyIA>();
        if(_enemyIA == null) {
            Debug.LogWarning("EnemyAttack: EnemyIA not found");
        }
    }

    // Update is called once per frame
    void Update() {
        AttackCooldown();
    }

    public void HandleAttack(PlayerHealth player) {
        player.DealDamage(_damage);
        _cooldownCounter = 0f;
    }

    private void AttackCooldown() {
        if(_cooldownCounter < _attackCooldown) {
            _cooldownCounter += Time.deltaTime;
        }
    }
    private bool CanAttack() {
        return (_cooldownCounter >= _attackCooldown);
    }

    private void OnCollisionStay(Collision collition) {
        if(collition.gameObject.tag.Equals("Player") && 
            CanAttack() && 
            _enemyIA.Status == EnemyStatus.OnAlert) {
            
            PlayerHealth health = collition.gameObject.GetComponent<PlayerHealth>();
            if(health == null) {
                Debug.LogWarning("Player Health is null");
                return;
            }
            
            HandleAttack(health);
        }
    }

    private void OnTriggerStay(Collider collider) {
        if(collider.gameObject.tag.Equals("Player")) {
            PlayerHealth health = collider.gameObject.GetComponent<PlayerHealth>();
            if(health == null) {
                Debug.LogWarning("Player Health is null");
                return;
            }
            if(CanAttack()) {
                HandleAttack(health);
            }
        }
    }
}
