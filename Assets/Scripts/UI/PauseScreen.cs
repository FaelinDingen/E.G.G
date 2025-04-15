using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private Singleton singleton;

    private void Awake()
    {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
    }
    public void Continue()
    {
        singleton.pauzer.togglePauze();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
