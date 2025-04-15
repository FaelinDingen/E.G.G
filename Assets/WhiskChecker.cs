using UnityEngine;

public class WhiskChecker : MonoBehaviour
{
    [SerializeField] private AudioSource whiskAudio;
    [SerializeField] private AudioSource normalAudio;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Whisk"))
        {
            whiskAudio.volume = 1;
            normalAudio.volume = 0;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Whisk"))
        {
            whiskAudio.volume = 0;
            normalAudio.volume = 1;
        }
    }
}
