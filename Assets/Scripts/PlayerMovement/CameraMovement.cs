using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Controlls")]
    [SerializeField] private Vector2 cameraPlayerOffset;
    [SerializeField] private float cameraSpeed;

    private Singleton singleton;
    private GameObject player;


    private void Awake() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        player = singleton.playerMovement.gameObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + cameraPlayerOffset.x, player.transform.position.z + cameraPlayerOffset.y);
        transform.position = Vector3.Lerp(transform.position, target, cameraSpeed);
    }
}
