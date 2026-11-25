using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Quest {
    [Header("Quest Data")]
    [SerializeField] private List<QuestItem> _questItems;

    public List<QuestItem> Items { get { return _questItems; } }
}
