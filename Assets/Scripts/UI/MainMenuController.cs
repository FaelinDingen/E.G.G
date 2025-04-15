using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void StartGame() {
        animator.SetTrigger("MenuAway");
        StartCoroutine(LoadGame());
    }

    public void Creddits() {
        animator.SetTrigger("Credits");
    }

    public void BackCredits() {
        animator.SetTrigger("CreditsBack");
    }

    public void Quit() {
        animator.SetTrigger("MenuAway");
        StartCoroutine(QuitGame());
    }

    private IEnumerator LoadGame() {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator QuitGame() {
        yield return new WaitForSeconds(.5f);
        Application.Quit();
    }
}
