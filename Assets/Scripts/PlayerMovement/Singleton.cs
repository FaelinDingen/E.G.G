using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;
    public PlayerMovement playerMovement;
    public CameraMovement cameraMovement;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }
}
