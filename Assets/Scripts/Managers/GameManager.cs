using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { InProgress, GameOver, OnPause }
public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    private static GameState _state = GameState.InProgress;
    private static int _previousSceneIndex;

    public static GameManager Instance {  get { return _instance; } }
    public int PreviousSceneIndex {  get { return _previousSceneIndex; } }

    public int PlayerDeaths { get; private set; }
    public int AlertCount { get; private set; }
    public float LevelCompletedTimeInSeconds {  get; private set; }

    public static GameState State {
        get         { return _state; }
        private set { _state = value; }
    }

    private void Awake () {
        if(_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
    /*
    private void OnEnable () {
        PlayerStateMachine.OnStateChangedToDead += GameOver;
    }
    private void OnDisable () {
        PlayerStateMachine.OnStateChangedToDead -= GameOver;
    }
    */
    public void LevelCompleted (string sceneName, float levelTime) => SceneManager.LoadScene(sceneName);

    public void GameOver () {
        State = GameState.GameOver;
        _previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadGameOver();
    }

    public void GameInProgress () {
        State = GameState.InProgress;
        Time.timeScale = 1f;
    }

    public void PauseGame() {
        State = GameState.OnPause;
        Time.timeScale = 0f;

        OpenPausePanel();
    }

    public void UnpauseGame () => GameInProgress();

    private void OpenPausePanel () {
        
    }

    public void LoadPreviousScene () => SceneManager.LoadScene(_previousSceneIndex);
    public void LoadMainMenu ()     => SceneManager.LoadScene("Main_Menu");
    private void LoadGameOver ()    => SceneManager.LoadScene("Game_Over");
}
