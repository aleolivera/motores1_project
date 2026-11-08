using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {
    private static int generatedId = 0;
    [Header("Door Data")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private List<KeyItem> _keyItems;
    [SerializeField] private bool _opened = false;

    [Header("Prefabs")]
    [SerializeField] private DoorManager _doorPrefab;
    [SerializeField] private WorldspaceUI _ui;

    SFXPlayer _sfxPlayer;
    Collider _collider;
    public bool IsOpened {
        get { return _opened; }
        private set { _opened = value; }
    }

    public List<KeyItem> Items { get { return _keyItems; } }
    public int Id { get { return _id; } private set { _id = value; } }
    public string Description { get { return _description; } }

    private void Awake () => Id = ++generatedId;

    private void Start () {
        _sfxPlayer = GetComponent<SFXPlayer>();
        _collider = GetComponent<Collider>();
        _doorPrefab = GetComponentInChildren<DoorManager>();
        _ui = GetComponentInChildren<WorldspaceUI>();


        if (_sfxPlayer == null) {
            Debug.LogError("Lock: SFXPlayer not found");
        }
        if (_doorPrefab == null) {
            Debug.LogError("Lock: DoorPrefab not found");
        }
        if (_ui == null) {
            Debug.LogError("Lock: World Space UI not found");
        }
    }
    public void Open () {
        IsOpened = true;
        _ui.enabled = false;
        _collider.enabled = false;

        _doorPrefab.Open();
        _sfxPlayer.PlaySound(0, 1f);
        NotificationUI.ShowNotification("Lock Opened: " + _name, Color.green);
    }
}
