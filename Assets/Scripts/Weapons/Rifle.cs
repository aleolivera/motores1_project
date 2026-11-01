using Unity.VisualScripting;
using UnityEngine;

public class Rifle : FireWeapon {
    [Header("Debug")]
    [SerializeField] private float _counter;
    [SerializeField] private bool _canShoot;
    SFXPlayer _sfxPlayer;

    private void Awake () {
        _counter = 0f;
        _canShoot = true;
    }

    private void Start () {
        _sfxPlayer = GetComponent<SFXPlayer>();
        if (_sfxPlayer == null) {
            Debug.LogError("Bullet: SFXPlayer not found");
        }
    }

    // Update is called once per frame
    void Update () {
        if(_counter > 0f)   _counter -= Time.deltaTime;
        else                _canShoot = true;
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
        _sfxPlayer.PlaySound(0, 1f);

        _canShoot = false;
        _counter = _shootCooldown;
    }
}
