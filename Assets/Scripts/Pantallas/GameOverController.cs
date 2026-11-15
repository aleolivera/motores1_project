
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void Reintentar()
    {
        GameManager.Instance.LoadPreviousScene();
    }

    public void VolverMenu() 
    {
        GameManager.Instance.LoadMainMenu();
    }
}
