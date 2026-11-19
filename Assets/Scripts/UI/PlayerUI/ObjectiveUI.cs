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

    private void HandleOnLevelStart (List<Quest> quests) {
        string objective = "";
        int counter = 1;
        foreach (Quest q in quests) {
            objective += counter + " - " + q.Description + "\n";
            counter++;
        }
        Debug.Log(objective);
        _txtObjectives.text = objective;
    }

    private void HandleOnQuestCompleted (List<Quest> quests) {
        string objective = "";
        foreach (Quest q in quests) {
            objective += q.Description + 
                        ((q.IsQuestCompleted) ? " (completed)" :  "") + 
                        "\n";
        }
        _txtObjectives.text = objective;
    }

}
