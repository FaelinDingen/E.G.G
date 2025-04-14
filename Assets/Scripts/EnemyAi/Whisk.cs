using UnityEngine;

public class Whisk : MonoBehaviour
{
    private Singleton singleton;
    private GameObject player;
    private Rigidbody rb;

    [SerializeField] private float whiskSpeed;

    private void Awake() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        player = singleton.playerMovement.gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 direction = (player.transform.position - gameObject.transform.position).normalized;
        rb.AddForce(direction * whiskSpeed);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            Debug.Log("PlayerDied");
        }
    }
}
