using Unity.VisualScripting;
using UnityEngine;

public class Rifle : FireWeapon {
    [Header("Debug")]
    [SerializeField] private float _counter;
    [SerializeField] private bool _canShoot;


    private void Awake () {
        _counter = 0f;
        _canShoot = true;
    }

    // Update is called once per frame
    void Update () {
        if(_counter > 0f) {
            _counter -= Time.deltaTime;
        } else {
            _canShoot = true;
        }
    }
    public override void Fire () {
        if (!_canShoot) return;

        Bullet bullet = BulletPool
                            .Instance
                            .GetObject()
                            .GetComponent<Bullet>();

        bullet.SetStartPosition(_spawnPoint.transform.position);
        bullet.SetStartRotation(_spawnPoint.transform.rotation);
        bullet.Launch(_launchVelocity);

        _canShoot = false;
        _counter = _shootCooldown;
    }
}
