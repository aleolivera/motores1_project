using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Health")]
    [SerializeField] private int _maxHealth = 300;
    [SerializeField] private int _health = 300;

    public int Health { 
        get         { return _health; } 
        private set { _health = value; } 
    }

    void Awake() {
        
    }
    void Start() {
        _health = _maxHealth;
    }

    void Update() {

    }

    public void RestoreFullHealth() {
        _health = _maxHealth;
    }

    public void RestoreHealth(int health) {
        _health += health;
        
        if (_health > _maxHealth) { _health = _maxHealth; }
    }

    public void DealDamage(int damage) {
        _health -= damage;
        if (_health < 0) { _health = 0; }
    }
}
