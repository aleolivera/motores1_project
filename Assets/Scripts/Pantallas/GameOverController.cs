
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Reintentar()
    {
        GameManager.Instance.LoadPreviousScene();
    }

    public void VolverMenu() 
    {
        GameManager.Instance.LoadMainMenu();
    }
}
