using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Health")]
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private int _maxHealth = 30;

    [Header("Debug")]
    [SerializeField] private int _health;

    public int Health { 
        get         { return _health; } 
        private set { _health = value; } 
    }
    public int MaxHealth {
        get         { return _maxHealth; }
        private set { _maxHealth = value; }
    }

    void Awake() {
        _health = _maxHealth;
    }
    void Start() {
        if(_healthBar == null)
            Debug.LogError("PlayerHealth: HealthBar not found");
    }

    public void RestoreHealth(int health) {
        _health += health;
        
        if (_health > _maxHealth) { _health = _maxHealth; }
        UpdateHeathBar();
    }

    public void DealDamage(int damage) {
        _health -= damage;
        if (_health < 0) { _health = 0; }

        UpdateHeathBar();
    }

    private void UpdateHeathBar() {
        _healthBar.SetHealth(_health, _maxHealth);
    }
}
