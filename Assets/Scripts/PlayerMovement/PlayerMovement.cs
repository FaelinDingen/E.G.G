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

    private void Awake() {
        rb = GetComponent<Rigidbody>();
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

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    IEnumerator jumpCooldown() { 
        checkingGround = false;
        yield return new WaitForSeconds(.2f);
        checkingGround = true;
    }
}
