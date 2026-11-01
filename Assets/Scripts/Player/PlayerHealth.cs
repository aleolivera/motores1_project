using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerHealth : MonoBehaviour {
    [Header("Health")]
    [SerializeField] private int _maxHealth = 30;

    public static event Action<int,int> OnHealthChange;
    CapsuleCollider _collider;
    SFXPlayer _sfxPlayer;

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

    void Awake() { _health = _maxHealth; }
    
    private void OnEnable () {
        PlayerInteractCollider.OnHealthItemCollected += HandleHealthItemCollected;
    }

    private void OnDisable () {
        PlayerInteractCollider.OnHealthItemCollected -= HandleHealthItemCollected;
    }

    private void Start () {
        _collider = GetComponent<CapsuleCollider>();
        if (_collider != null) { InitCollider(); }

        _sfxPlayer = GetComponent<SFXPlayer>();
        if (_sfxPlayer == null) {
            Debug.LogError("PlayerHealth: SFXPlayer not found");
        }
    }

    private void HandleHealthItemCollected (HealthItem item) {
        if (item == null) return;
        RestoreHealth(item.Health);
    }

    public void RestoreHealth(int health) {
        if (health < 0) return; 
        
        _health += health;
        if (_health > _maxHealth) { _health = _maxHealth; }
        
        OnHealthChange?.Invoke(_health, _maxHealth);
    }

    public void DealDamage(int damage) {
        if (damage < 0) return;
        
        _health -= damage;
        if (_health <= 0) { 
            _health = 0;
            PlayerDead();
        }

        OnHealthChange?.Invoke(_health, _maxHealth);
    }
    
    public void PlayerDead() {
        gameObject.layer = 0;
        _collider.enabled = false;

        PlayDeadSfx();
        PlayerStateMachine.Instance.CurrentState = PlayerState.Dead;
    }

    private void InitCollider () {
        const int y = 1;
        _collider.isTrigger = false;
        _collider.center = Vector3.up;
        _collider.radius = 0.28f;
        _collider.height = 2.11f;
        _collider.direction = y;
    }

    public void PlayDamageSfx () => _sfxPlayer.PlaySound(1);
    public void PlayDeadSfx () => _sfxPlayer.PlaySound(2);
    
}
