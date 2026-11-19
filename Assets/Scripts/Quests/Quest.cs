using UnityEngine;

public abstract class Quest : MonoBehaviour{
    private static int generatedId = 0;
    [Header("Quest Data")]
    [SerializeField] protected int _id;
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;

    [SerializeField] protected bool _questCompleted = false;
    public bool IsQuestCompleted {
        get { return _questCompleted; }
        set { _questCompleted = value; }
    }

    public int Id { get { return _id; } }
    public string QuestName { get { return _name; } }
    public string Description { get { return _description; } }

    private void Awake () => _id = ++generatedId;
    
}
