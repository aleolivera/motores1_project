using UnityEngine;

public abstract class Item : MonoBehaviour {
    [Header("Item Settings")]
    private static int generatedId = 0;
    [SerializeField] protected int _id;
    [SerializeField] protected string _name;

    private void Awake () {
        _id = ++generatedId;
    }

    public int Id {
        get { return _id; }
        private set { _id = value; }
    }
    public string Name {
        get { return _name; }
        private set { _name = value; }
    }
}
