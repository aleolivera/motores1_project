using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { InProgress, GameOver }
public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    private static GameState _state = GameState.InProgress;
    private static int _playerDeaths = 0;

    public static GameManager Instance {  get { return _instance; } }
    public static GameState State {
        get         { return _state; }
        private set { _state = value; }
    }
    public static int PlayerDeaths {
        get         { return _playerDeaths; }
        private set { _playerDeaths = value; }
    }

    private void Awake () {
        if(_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable () {
        PlayerStateMachine.OnStateChangedToDead += GameOver;
    }
    private void OnDisable () {
        PlayerStateMachine.OnStateChangedToDead -= GameOver;
    }


    public void LoadNextLevel (string name) {
        SceneManager.LoadScene(name);
    }
    private void GameOver () {
        PlayerDeaths++;
        State = GameState.GameOver;
    }
    private void GameInProgress () {
        State = GameState.InProgress;
    }

}
