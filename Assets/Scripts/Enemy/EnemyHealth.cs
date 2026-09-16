using UnityEngine;

[RequireComponent(typeof(EnemyIA))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 30;
    [SerializeField] private int _health;

    EnemyIA _enemyIA;

    public int Health {
        get         { return _health; }
        private set { _health = value; }
    }

    public EnemyStatus Status {
        get { return _enemyIA.Status; }
    }

    void Awake() {
        _health = _maxHealth;
    }
    void Start() {
        _enemyIA = GetComponent<EnemyIA>();
        if(_enemyIA == null) {
            Debug.LogWarning("EnemyHealth: EnemyIA not found");
        }
    }

    public void RestoreFullHealth() { 
        _health = _maxHealth; 
    }

    public void RestoreHealth(int health) {
        _health += health;

        if(_health > _maxHealth) { _health = _maxHealth; }
    }

    public void DealDamage(int damage) {
        _health -= damage;
        if(_health <= 0) { 
            _health = 0;
            KnockOut();
        }
    }

    public void KnockOut() {
        _enemyIA.DisableEnemy();
    }
}
