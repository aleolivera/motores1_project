using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState { InProgress, PlayingCinematic }
public class LevelManager : MonoBehaviour {
    private static LevelManager _instance;
    
    [Header("Quest Givers")]
    [SerializeField] private List<Quest> _quests = new List<Quest>();
    [SerializeField] private string _nextSceneName;

    [Header("Debug")]
    private LevelState _state;
    
    //PROPERTIES
    public static LevelManager Instance { get { return _instance; } }
    public int SceneBuildIndex      { get; set; }
    public float LevelTimeCounter   { get; private set; }
    public static int PlayerDeaths  { get; private set; }
    public LevelState State { 
        get { return _state; } 
        private set { _state = value; } 
    }
    
    //EVENTS
    public static event Action<List<Quest>> OnLevelStart;
    public static event Action<List<Quest>,int> OnQuestCompleted;


    //MONOBEHAVIOUR METHODS
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
        if (_quests == null)
            Debug.LogWarning("LevelManager: Quests list is null.");
        else if (_quests.Count == 0)
            Debug.LogWarning("LevelManager: Quests are empty.");
        else {
            bool b = false;
            foreach(Quest quest in _quests) {
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
    
    //METHODS
    public void QuestCompleted(int questId) {
        OnQuestCompleted?.Invoke(_quests, questId);

        foreach (Quest quest in _quests) { 
            if(!quest.IsQuestCompleted) return;   
        }
        LevelCompleted();
    }

    public void LevelCompleted () => GameManager.Instance.LevelCompleted(_nextSceneName, LevelTimeCounter);
    public static void RestartLevel () => GameManager.Instance.RestartLevel();
    public static void GameOver () {
        PlayerDeaths++;
        GameManager.Instance.GameOver();
    }
}
