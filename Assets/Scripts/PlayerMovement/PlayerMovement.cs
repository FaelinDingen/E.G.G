using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 1f;

    [HideInInspector] public Rigidbody rb;
    private bool checkingGround = true;
    [HideInInspector] public bool grounded;
    private Singleton singleton;
    private GameObject camera;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        camera = singleton.cameraMovement.gameObject;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, groundCheckDistance) && checkingGround) {
            grounded = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(new Vector3(0, jumpForce, 0));
            grounded = false;
            StartCoroutine(jumpCooldown());
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // getting angle
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraRight * moveHorizontal + cameraForward * moveVertical).normalized;

        rb.AddForce(movement * speed);
    }

    IEnumerator jumpCooldown() { 
        checkingGround = false;
        yield return new WaitForSeconds(.2f);
        checkingGround = true;
    }
}
