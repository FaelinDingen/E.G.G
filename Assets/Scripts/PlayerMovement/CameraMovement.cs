using UnityEngine;
using UnityEngine.Diagnostics;

public class CameraMovement : MonoBehaviour {
    [Header("Camera Controlls")]
    [SerializeField] private Vector2 cameraPlayerOffset;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float groundedCameraSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxVerticalCameraRotation;
    [SerializeField] private float minVerticalCameraRotation;
    [SerializeField] private float cameraHoverDistance;

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
        currentVerticalRotationAngle = Mathf.Clamp(currentVerticalRotationAngle, minVerticalCameraRotation, maxVerticalCameraRotation);
    }

    private void FixedUpdate() {
        Quaternion rotation = Quaternion.Euler(currentVerticalRotationAngle, currentHorizontalRotationAngle, 0);
        Vector3 offset = rotation * new Vector3(cameraPlayerOffset.y, cameraPlayerOffset.x, -cameraPlayerOffset.y);
        Vector3 targetPosition = player.transform.position + offset;

        

        transform.position = Vector3.Lerp(transform.position, targetPosition, currentCameraSpeed);
        //check for ground under camera
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float terrainHeight = hit.point.y;
            if (transform.position.y < terrainHeight + cameraHoverDistance) {
                transform.position = new Vector3(transform.position.x, terrainHeight + cameraHoverDistance, transform.position.z);
            }
        }
        transform.LookAt(player.transform.position + Vector3.up * cameraPlayerOffset.x);
    }
}
