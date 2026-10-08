using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {
    private static int generatedId = 0;
    [Header("Quest Data")]
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private List<QuestItem> _questItems;
    [SerializeField] private bool _questCompleted = false;
    
    public bool IsQuestCompleted { 
        get { return _questCompleted;  } 
        set { _questCompleted = value; } 
    }

    public List<QuestItem> Items { get { return _questItems; } }
    public int Id { get { return _id; } }
    public string Description { get { return _description; } }

    private void Awake () {
        _id = ++generatedId;
    }
    
}
