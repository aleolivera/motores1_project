using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerMeleeWeapon : MonoBehaviour {
    [Header("Weapon settings")]
    [SerializeField] private int _damage = 10;
    // PlayerAttack _attack;

    public static event Action<EnemyHealth,int> OnEnemyHit;

    BoxCollider _collider;

    public bool ColliderEnable { 
        get { return _collider.enabled; } 
        set { _collider.enabled = value; } 
    }

    void Start () {
        //_attack     = GetComponentInParent<PlayerAttack>();
        _collider   = GetComponent<BoxCollider>();

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
            //_attack.DealDamage(enemy, _damage);
            OnEnemyHit?.Invoke(enemy,_damage);
        }
    }

}
