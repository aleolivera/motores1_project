using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Pool<GameObject> {
    private static BulletPool _instance;
    public static BulletPool Instance { get { return _instance; } }

    public void Awake () {
        if(_instance == null) {  
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            _instance = null;
        }
    }

    private void Start () {
        InitiatePool();
    }

    override protected void InitiatePool () {
        if (_pool.Count > 0) return;

        for (int i = 0; i < _capacity; i++) {
            GameObject bullet = Instantiate(_prefab, transform.position, transform.rotation);
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }

    override public GameObject GetObject () {
        GameObject bullet;
        if (_pool.Count == 0) {
            bullet = Instantiate(_prefab, transform.position, transform.rotation);
            _capacity++;
        } else {
            bullet = _pool.Dequeue();
        }

        bullet.gameObject.SetActive(true);
        return bullet;
    }

    override public void ReturnObject (GameObject obj) {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
