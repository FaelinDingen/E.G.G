using UnityEngine;

public class WhiskChecker : MonoBehaviour
{
    [SerializeField] private AudioSource whiskAudio;
    [SerializeField] private AudioSource normalAudio;
    private int whiskCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Whisk"))
        {
            whiskCount++;
        }
        if (whiskCount > 0) {
            whiskAudio.volume = 1;
            normalAudio.volume = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Whisk"))
        {
            whiskCount--;
        }
        if (whiskCount < 1) {
            whiskAudio.volume = 0;
            normalAudio.volume = 1;
        }
    }
}
