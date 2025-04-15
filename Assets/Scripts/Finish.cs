using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    [SerializeField] private Animator fadeout;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadeout.SetTrigger("Fade");
            StartCoroutine(SceneChange());
        }
    }

    private IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main");
    }
}
