using UnityEngine;

public class PausePanelUI : MonoBehaviour {
    [SerializeField] private GameObject _panel;

    private void OnEnable () => GameManager.OnGamePaused += OpenPausePanel;
    private void OnDisable () => GameManager.OnGamePaused -= OpenPausePanel;

    public void OpenPausePanel () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _panel.SetActive(true);
    }

    public void ClosePausePanel () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _panel.SetActive(false);

        GameManager.Instance.UnpauseGame();
    }

    public void BackToMain () => GameManager.Instance.LoadMainMenu();

}
