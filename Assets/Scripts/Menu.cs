using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MomentumScene1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
