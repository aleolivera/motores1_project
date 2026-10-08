using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    private static int generatedId = 0;
    [Header("Door Data")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private List<KeyItem> _keyItems;
    [SerializeField] private bool _opened = false;

    public bool IsOpened {
        get { return _opened; }
        private set { _opened = value; }
    }

    public List<KeyItem> Items { get { return _keyItems; } }
    public int Id { get { return _id; } private set { _id = value; } }
    public string Description { get { return _description; } }

    private void Awake () {
        Id = ++generatedId;
    }

    public void Open () {
        IsOpened = true;
        gameObject.SetActive(false);
    }
}
