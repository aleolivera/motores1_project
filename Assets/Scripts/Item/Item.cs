using UnityEngine;

public abstract class Item : MonoBehaviour {
    [Header("Item Settings")]
    private static int generatedId = 0;
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    
    [Header("Debug")]
    [SerializeField] protected int _id;
    private void Awake () => _id = ++generatedId;
    

    public int Id {
        get { return _id; }
        private set { _id = value; }
    }
    public string Name {
        get { return _name; }
        private set { _name = value; }
    }
    public string Description {
        get { return _description; }
        private set { _description = value; }
    }
}
