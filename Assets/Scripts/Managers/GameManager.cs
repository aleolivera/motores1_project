using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { InProgress, GameOver, OnPause }
public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    private GameState _state = GameState.InProgress;

    public static GameManager Instance {  get { return _instance; } }
    public GameState State {
        get { return _state; }
        private set { _state = value; }
    }
    public int PreviousSceneBuildIndex { get; private set; }

    // -------------- EVENTS
    public static event Action OnGameInProgress;
    public static event Action<PlayerInventory> OnGamePaused;
    public static event Action OnGameOver;

    // -------------- GAMEPLAY STADISTICS
    public int PlayerDeathCount     { get; private set; }
    public int EnemyDisabledCount   { get; private set; }
    public int AlertCount           { get; private set; }
    public float LevelCompletedTime {  get; private set; }

    

    private void Awake () {
        if(_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
    private void OnEnable () {
        EnemyDetection.OnPlayerDetected         += IncrementAlertCount;
        EnemyDetection.OnEnemyDisabled          += IncrementEnemyDisableCount;
        PlayerStateMachine.OnStateChangedToDead += IncrementDeathCount;
    }
    private void OnDisable () {
        EnemyDetection.OnPlayerDetected         -= IncrementAlertCount;
        EnemyDetection.OnEnemyDisabled          -= IncrementEnemyDisableCount;
        PlayerStateMachine.OnStateChangedToDead -= IncrementDeathCount;
    }
 
    public void LevelCompleted (string sceneName, float levelTime) {
        LevelCompletedTime = levelTime;
        SceneManager.LoadScene(sceneName);
    } 

    public void GameOver () {
        State = GameState.GameOver;
        OnGameOver?.Invoke();
        PreviousSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadGameOver();
    }

    public void GameInProgress () {
        Time.timeScale = 1f;
        State = GameState.InProgress;
        
        OnGameInProgress?.Invoke();
    }

    public void PauseGame(PlayerInventory inventory) {
        //Time.timeScale = 0f;
        State = GameState.OnPause;
        OnGamePaused?.Invoke(inventory);
    }

    public void UnpauseGame () => GameInProgress();


    public void LoadPreviousScene () => SceneManager.LoadScene(PreviousSceneBuildIndex);
    private void LoadGameOver () => SceneManager.LoadScene("Game_Over");
    public void LoadMainMenu () {
        ClearGameStadistics();
        SceneManager.LoadScene("Main_Menu");
    }
    public void RestartLevel () {
        ClearGameStadistics();
        SceneManager.GetActiveScene();
    }

    private void IncrementAlertCount () => AlertCount++;
    private void IncrementDeathCount () => PlayerDeathCount++;
    private void IncrementEnemyDisableCount () => EnemyDisabledCount++;

    private void ClearGameStadistics () {
        PlayerDeathCount = 0;
        EnemyDisabledCount = 0;
        AlertCount = 0;
        LevelCompletedTime = 0f;
    }
}
