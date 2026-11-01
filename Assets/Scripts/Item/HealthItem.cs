using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthItem : MonoBehaviour {
    [Header("Health settings")]
    [SerializeField] private int _health = 100;

    Collider _collider;
    SFXPlayer _sfxPlayer;
    public int Health { get {return _health; } }

    private void Start () {
        _collider = GetComponent<Collider>();
        _sfxPlayer = GetComponent<SFXPlayer>();

        if(_collider == null) {
            Debug.LogError("HealthItem: Collider not found");
        }
        if(_sfxPlayer == null) {
            Debug.LogError("HealthItem: SFX Player not found");
        }
        _collider.enabled = true;
        _collider.isTrigger = true;
    }

    public void PlayHealthItemCollectedSfx () => _sfxPlayer.PlaySound(0,1f);
    public void Destroy () => gameObject.SetActive(false);
}
