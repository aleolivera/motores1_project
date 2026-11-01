using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerMeleeWeapon : MonoBehaviour {
    [Header("Weapon settings")]
    [SerializeField] private int _damage = 10;

    public static event Action<EnemyHealth,int> OnEnemyHit;

    BoxCollider _collider;
    SFXPlayer _sfxPlayer;

    public bool ColliderEnable { 
        get { return _collider.enabled; } 
        set { _collider.enabled = value; } 
    }

    void Start () {
        _collider   = GetComponent<BoxCollider>();
        _sfxPlayer = GetComponentInParent<SFXPlayer>();

        if(_sfxPlayer == null) {
            Debug.LogError("PlayerMeleeWeapon: SFXPlayer was not found");
        }

        _collider.isTrigger = true;
        _collider.enabled   = false;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Enemy")){
            EnemyStateMachine enemyState = other.GetComponent<EnemyStateMachine>();
            if (enemyState.State == EnemyActionState.Unconscious ||
                enemyState.State == EnemyActionState.Unconscious) {
                return;
            } 
            
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            PlayAttackHitSfx();
            
            OnEnemyHit?.Invoke(enemy,_damage);
        }
    }
    /*
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            EnemyStateMachine enemyState = collision.gameObject.GetComponent<EnemyStateMachine>();
            if (enemyState.State == EnemyActionState.Unconscious ||
                enemyState.State == EnemyActionState.Unconscious) {
                return;
            }

            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            PlayAttackHitSfx();

            OnEnemyHit?.Invoke(enemy, _damage);
        }
    }
    */
    void PlayAttackHitSfx () => _sfxPlayer.PlaySound(3, 1f);
}
