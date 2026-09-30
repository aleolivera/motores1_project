using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))] 
[RequireComponent(typeof(Collider))]

public class Bullet : MonoBehaviour , IProjectile{
    [SerializeField] private float _counter;
    [SerializeField] private int _damage = 20;
    private float _lifeTime = 2f;
    Rigidbody _rigidBody;

    private void Awake () {
        _counter    = _lifeTime;
        _rigidBody  = GetComponent<Rigidbody>();
    }

    private void OnEnable () {
        SetStartPosition(Vector3.up * 2f);
        SetStartRotation(Quaternion.identity);
        Launch(40f);
    }

    void Update () {
        if(_counter > 0f) {
            _counter -= Time.deltaTime;
        } else {
            _counter = _lifeTime;
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter (Collision collider) {
        if (collider.gameObject.CompareTag("Player")) {
            PlayerHealth player = collider.gameObject.GetComponent<PlayerHealth>();
            if (player == null) return;

            player.DealDamage(_damage);
            DestroyProjectile();
        }
    }
    public void SetStartPosition(Vector3 position) {
        transform.position = position;
    }

    public void SetStartRotation(Quaternion rotation) {
        transform.rotation = rotation;
    }

    public void Launch(float speed) {
        Vector3 target = transform.forward;
        _rigidBody.linearVelocity   = target * speed;
        _rigidBody.angularVelocity  = Vector3.zero;
    }

    public void Reset() {
        transform.position          = Vector3.zero;
        _rigidBody.angularVelocity  = Vector3.zero;
        _rigidBody.linearVelocity   = Vector3.zero;
    }

    public void DestroyProjectile () {
        Reset();
        BulletPool.Instance.ReturnObject(gameObject);
    }
}
