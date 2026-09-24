using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyIA))]
public class EnemyHealth : MonoBehaviour {
    [Header("Health")]
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private int _maxHealth = 30;
    
    [Header("Debug")]
    [SerializeField] private int _health;

    EnemyIA _enemyIA;

    public int Health {
        get { return _health; }
        private set { _health = value; }
    }
    public int MaxHealth {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }

    void Awake() {
        _health = _maxHealth;
    }
    void Start() {
        _enemyIA = GetComponent<EnemyIA>();
        _healthBar = GetComponentInChildren<HealthBar>();

        if(_enemyIA == null) {
            Debug.LogError("EnemyHealth: EnemyIA not found");
        }
        if(_healthBar == null) {
            Debug.LogError("EnemyHealth: HealthBar not found");
        }
    }

    public void RestoreHealth(int health) {
        _health += health;
        if(_health > _maxHealth) { _health = _maxHealth; }

        UpdateHeathBar();
    }

    public void DealDamage(int damage) {
        if(_enemyIA.DetectionState == DetectionState.OnPatrol) {
            KnockOut();
            UpdateHeathBar();
            return;
        }

        _health -= damage;
        if(_health <= 0) {
            _health = 0;
            KnockOut();
        }

        UpdateHeathBar();
    }

    public void KnockOut() {
        _enemyIA.DisableEnemy();
    }

    private void UpdateHeathBar() {
        _healthBar.SetHealth(_health, _maxHealth);
    }
}