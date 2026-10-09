using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthItem : MonoBehaviour {
    [Header("Health settings")]
    [SerializeField] private int _health = 100;

    Collider _collider;
    public int Health { get {return _health; } }

    private void Start () {
        _collider = GetComponent<Collider>();
        if(_collider == null) {
            Debug.LogError("HealthItem: Collider not found");
        }
        _collider.enabled = true;
        _collider.isTrigger = true;
    }

    public void Destroy () {
        gameObject.SetActive(false);
    }
}
