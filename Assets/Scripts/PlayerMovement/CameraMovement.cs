using UnityEngine;
using UnityEngine.Diagnostics;

public class CameraMovement : MonoBehaviour {
    [Header("Camera Controlls")]
    [SerializeField] private Vector2 cameraPlayerOffset;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float groundedCameraSpeed;
    [SerializeField] private float rotationSpeed;

    private Singleton singleton;
    private GameObject player;
    private PlayerMovement playerMovement;

    private float currentCameraSpeed;
    private float currentHorizontalRotationAngle;
    private float currentVerticalRotationAngle;


    private void Awake() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        player = singleton.playerMovement.gameObject;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update() {
        currentCameraSpeed = (playerMovement.grounded ? groundedCameraSpeed : cameraSpeed);

        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");
        currentHorizontalRotationAngle += horizontalInput * rotationSpeed * Time.deltaTime;
        currentVerticalRotationAngle += verticalInput * rotationSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(Mathf.Clamp(currentVerticalRotationAngle, -40,80), currentHorizontalRotationAngle, 0);
        Vector3 offset = rotation * new Vector3(cameraPlayerOffset.y, cameraPlayerOffset.x, -cameraPlayerOffset.y);
        Vector3 targetPosition = player.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, currentCameraSpeed);
        transform.LookAt(player.transform.position + Vector3.up * cameraPlayerOffset.x);
    }
}
