using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [Header("Attack Settings")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _normalDamage = 10;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private Rifle _weapon;

    public float AttackRange { get { return _attackRange; } }

    EnemyIA _enemyIA;

    private void Awake() {
        _damage = _normalDamage;
    }
    void Start() {
        _enemyIA = GetComponent<EnemyIA>();


        if(_enemyIA == null) {
            Debug.LogWarning("EnemyAttack: EnemyIA not found");
        }
        if(_weapon == null) {
            Debug.LogWarning("EnemyAttack: Weapon not found");
        }
    }

    public void HandleAttack () {
        _weapon.Fire();
    }
}