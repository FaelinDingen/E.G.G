using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rb;
    private bool grounded;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        //if (Physics.Raycast(gameObject.transform.position, Vector3.down))

        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(new Vector3(0,jumpForce,0));
        }
    }
}
