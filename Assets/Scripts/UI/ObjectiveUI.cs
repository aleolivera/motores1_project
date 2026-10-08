using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ObjectiveUI : MonoBehaviour {
    [Header("Text refs")]
    [SerializeField] TextMeshProUGUI _txtObjectives;

    private void OnEnable () {
        LevelManager.OnLevelStart += HandleOnLevelStart;
        LevelManager.OnQuestCompleted += HandleOnQuestCompleted;
    }

    private void OnDisable () {
        LevelManager.OnLevelStart -= HandleOnLevelStart;
        LevelManager.OnQuestCompleted -= HandleOnQuestCompleted;
    }

    private void Start () {
        if (_txtObjectives == null) {
            Debug.LogError("ObjectiveUI: TextMesh ref not found");
        }
    }

    private void HandleOnLevelStart (List<QuestGiver> quests) {
        string objective = "";
        foreach (QuestGiver q in quests) {
            objective += q.Description + "\n";
        }
        _txtObjectives.text = objective;
    }

    private void HandleOnQuestCompleted (List<QuestGiver> quests) {
        string objective = "";
        foreach (QuestGiver q in quests) {
            objective += q.Description + 
                        ((q.IsQuestCompleted) ? " (completed)" :  "") + 
                        "\n";
        }
        _txtObjectives.text = objective;
    }

}
