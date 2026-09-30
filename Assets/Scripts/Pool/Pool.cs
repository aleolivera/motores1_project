using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool <T>: MonoBehaviour, IPoolable<T> {
    [Header("Pool Settings")]
    [SerializeField] protected int _capacity = 5;
    [SerializeField] protected T _prefab;
    [SerializeField] protected Queue<T> _pool = new Queue<T>();

    private void Start () {
        if(_prefab == null) {
            Debug.LogError("Pool: No prefab found.");
        }
    }

    protected abstract void InitiatePool ();
    public abstract T GetObject ();
    public abstract void ReturnObject (T obj);
}
