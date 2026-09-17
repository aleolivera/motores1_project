using UnityEngine;

public class KeyItem : MonoBehaviour {
    [Header("Key Item Data")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;

    public int Id { 
        get { return _id; } 
        private set { _id = value; } 
    }
    public string Name { 
        get { return _name; } 
        private set { _name = value; } 
    }

    void Start() { }

    // Update is called once per frame
    void Update() { }

}
