using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;
    public PlayerMovement playerMovement;
    public CameraMovement cameraMovement;
    public AudioSource mainAudioSource;
    public GameObject whiskPopup;
    public GameObject WhiskPopupButtons;
    public Pauzer pauzer;
    public GameObject pauseScreen;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }
}
