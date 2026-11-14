
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void Reintentar()
    {
        SceneManager.LoadScene("Main_GameOver");
    }

    public void VolverMenu() 
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
