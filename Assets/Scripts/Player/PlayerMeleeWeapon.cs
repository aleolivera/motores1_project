using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerMeleeWeapon : MonoBehaviour {
    [Header("Weapon settings")]
    [SerializeField] private int _damage = 10;
    PlayerAttack _attack;
    BoxCollider _collider;

    public BoxCollider Collider { get { return _collider; } }

    void Start () {
        _attack     = GetComponentInParent<PlayerAttack>();
        _collider   = GetComponent<BoxCollider>();

        _collider.isTrigger = true;
        _collider.enabled   = false;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Enemy")){
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            _attack.DealDamage(enemy, _damage);
        }
    }

    public void EnableCollider (bool enabled) {
        _collider.enabled = enabled;
    }
}
