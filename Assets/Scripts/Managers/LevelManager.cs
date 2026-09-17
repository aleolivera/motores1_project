using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [Header("Quest Givers")]
    [SerializeField] private List<QuestGiver> _quests;
    [SerializeField] private string _nextSceneName;

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        }
    }

    void Start() {
        if(_quests.Count == 0) {
            Debug.LogWarning("LevelManager: Quests are empty.");
        }
    }

    public void QuestCompleted(int questId) {
        foreach(QuestGiver quest in _quests) { 
            if(!quest.IsQuestCompleted) return;   
        }
        LevelCompleted();
    }

    public void LevelCompleted() {
        if(SceneManager.GetSceneByName(_nextSceneName).IsValid()) {
            SceneManager.LoadScene(_nextSceneName);
        } else {
            Debug.LogError("LevelManager: Scene " + _nextSceneName + " is not valid");
        }

    }
}
