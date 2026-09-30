using Unity.VisualScripting;
using UnityEngine;

public abstract class FireWeapon : MonoBehaviour, IFireWeapon {

    [Header("Fire Weapon Settings")]
    [SerializeField] protected GameObject _spawnPoint;
    [SerializeField] protected float _shootCooldown = 1f;
    [SerializeField] protected float _launchVelocity = 40f;

    private void Start () {
        if (_spawnPoint == null) {
            Debug.LogError("FireWeapon: Spawn point not found.");
        }
    }

    abstract public void Fire ();
}
