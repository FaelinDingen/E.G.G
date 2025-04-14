using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [Header("Camera Controlls")]
    [SerializeField] private Vector2 cameraPlayerOffset;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float groundedCameraSpeed;

    private Singleton singleton;
    private GameObject player;
    private PlayerMovement playerMovement;

    private float currentCameraSpeed;


    private void Awake() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        player = singleton.playerMovement.gameObject;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update() {
        currentCameraSpeed = (playerMovement.grounded ? groundedCameraSpeed : cameraSpeed);

        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + cameraPlayerOffset.x, player.transform.position.z + cameraPlayerOffset.y);
        transform.position = Vector3.Lerp(transform.position, target, currentCameraSpeed);
    }
}
