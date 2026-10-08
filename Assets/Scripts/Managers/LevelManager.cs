using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [Header("Quest Givers")]
    [SerializeField] private List<QuestGiver> _quests = new List<QuestGiver>();
    [SerializeField] private string _nextSceneName;

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public static event Action<List<QuestGiver>> OnLevelStart;
    public static event Action<List<QuestGiver>> OnQuestCompleted;
    public static event Action OnLevelCompleted;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    void Start() {
        if (_quests == null) {
            Debug.LogWarning("LevelManager: Quests list is null.");
        } else if (_quests.Count == 0) {
            Debug.LogWarning("LevelManager: Quests are empty.");
        } else {
            bool b = false;
            foreach(QuestGiver item in _quests) {
                if (item == null)   Debug.LogWarning("LevelManager: Quest item is null.");
                else                b = true;
            }
            if(b) OnLevelStart?.Invoke(_quests);
        }
    }

    public void QuestCompleted(int questId) {
        OnQuestCompleted?.Invoke(_quests);
        foreach(QuestGiver quest in _quests) { 
            if(!quest.IsQuestCompleted) return;   
        }
        LevelCompleted();
    }

    public void LevelCompleted() {
        OnLevelCompleted?.Invoke();

        if(SceneManager.GetSceneByName(_nextSceneName).IsValid()) {
            GameManager.Instance.LoadNextLevel(_nextSceneName);
        } else {
            Debug.LogError("LevelManager: Scene " + _nextSceneName + " is not valid");
        }
    }

    public static void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
