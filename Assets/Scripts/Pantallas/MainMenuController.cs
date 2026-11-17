
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject controlsPanel;
    [SerializeField] private string _playLevelName;

    private void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        if (_playLevelName.Equals("")) {
            Debug.LogError("MainMenuController: Level name not found.");
            return;
        }
        
        SceneManager.LoadScene(_playLevelName);
    }

    public void OpenControls() => controlsPanel.SetActive(true);
    public void CloseControls() => controlsPanel.SetActive(false);
}
