using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [Header("Quest Givers")]
    [SerializeField] private List<QuestGiver> _quests = new List<QuestGiver>();
    [SerializeField] private string _nextSceneName;

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public int SceneBuildIndex { get; set; }
    public static event Action<List<QuestGiver>> OnLevelStart;
    public static event Action<List<QuestGiver>> OnQuestCompleted;

    public float LevelTimeCounter { get; private set; }
    public static int PlayerDeaths { get; private set; }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void OnEnable () => PlayerStateMachine.OnStateChangedToDead += GameOver;
    
    private void OnDisable () => PlayerStateMachine.OnStateChangedToDead -= GameOver;
    

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (_quests == null)
            Debug.LogWarning("LevelManager: Quests list is null.");
        else if (_quests.Count == 0)
            Debug.LogWarning("LevelManager: Quests are empty.");
        else {
            bool b = false;
            foreach(QuestGiver quest in _quests) {
                if (quest == null)   Debug.LogWarning("LevelManager: Quest is null.");
                else                b = true;
            }
            if(b) OnLevelStart?.Invoke(_quests);
        }

        SceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        LevelTimeCounter    = 0f;
        PlayerDeaths        = 0;
    }

    private void Update () => LevelTimeCounter += Time.deltaTime;
    
    public void QuestCompleted(int questId) {
        string questName = _quests.First(quest => quest.Id == questId).name;
        
        OnQuestCompleted?.Invoke(_quests);
        NotificationUI.ShowNotification("Quest Completed!: " + questName , Color.green);

        foreach (QuestGiver quest in _quests) { 
            if(!quest.IsQuestCompleted) return;   
        }
        LevelCompleted();
    }

    public void LevelCompleted () {
        GameManager.Instance.LevelCompleted(_nextSceneName, LevelTimeCounter);
    }

    public static void RestartLevel () => GameManager.Instance.RestartLevel();
    
    public static void GameOver () {
        PlayerDeaths++;
        GameManager.Instance.GameOver();
    }
}
