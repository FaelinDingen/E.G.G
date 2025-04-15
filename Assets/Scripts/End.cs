using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private Singleton singleton;

    private void Awake() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DataCarrier.finishedTime = singleton.playerMovement.timer;
            SceneManager.LoadScene("Ending screen");
        }
    }
}
