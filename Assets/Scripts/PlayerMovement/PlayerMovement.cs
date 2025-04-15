using System.Collections;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float maxGravity = 20;
    [SerializeField] private float minGravity = 0;
    [SerializeField] private float transitionTime;
    [SerializeField] private Vector3 heavyColor;
    [SerializeField] private GameObject shards;
    private float currentGravity = 0;

    [HideInInspector] public Rigidbody rb;
    private bool checkingGround = true;
    [HideInInspector] public bool grounded;
    private Singleton singleton;
    private GameObject cameraObject;
    private Coroutine changeGravity;
    private Vector3 groundNormal;
    private MeshRenderer meshRenderer;
    private Material playerMaterial;
    private SphereCollider sphereCollider;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        cameraObject = singleton.cameraMovement.gameObject;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        playerMaterial = meshRenderer.material;
        sphereCollider = gameObject.GetComponent<SphereCollider>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, groundCheckDistance)) {
            if (checkingGround) {
                grounded = true;
            }
        }
        else {
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(new Vector3(0, jumpForce, 0));
            grounded = false;
            StartCoroutine(jumpCooldown());
        }

        if (Input.GetMouseButtonDown(0)) {
            if (changeGravity != null) {
                StopCoroutine(changeGravity);
            }
            changeGravity = StartCoroutine(ChangeGravity(maxGravity));
        }
        if (Input.GetMouseButtonUp(0)) {
            if (changeGravity != null) {
                StopCoroutine(changeGravity);
            }
            changeGravity = StartCoroutine(ChangeGravity(minGravity));
        }
    }

    private void FixedUpdate() {
        if (grounded) {
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, groundCheckDistance)) {
                groundNormal = hit.normal;
            }
            else {
                groundNormal = Vector3.zero;
            }
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // getting angle
        Vector3 cameraForward = cameraObject.transform.forward;
        Vector3 cameraRight = cameraObject.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraRight * moveHorizontal + cameraForward * moveVertical).normalized;
        Vector3 projectedMovement = Vector3.ProjectOnPlane(movement, groundNormal);
        //debug ray to show the direction of the applied velocity
        Debug.DrawRay(transform.position, projectedMovement * 5, Color.red, 25);
        rb.AddForce(projectedMovement * speed);

        Vector3 customGravity = new Vector3(0, -currentGravity, 0);
        Vector3 projectedGravity = Vector3.ProjectOnPlane(customGravity, groundNormal);
        rb.AddForce(projectedGravity, ForceMode.Acceleration);
    }

    public void Die() {
        meshRenderer.enabled = false;
        sphereCollider.enabled = false;
        rb.useGravity = false;

        shards.SetActive(true);
        StartCoroutine(LoadLevel());
    }

    IEnumerator jumpCooldown() {
        checkingGround = false;
        yield return new WaitForSeconds(.2f);
        checkingGround = true;
    }

    IEnumerator ChangeGravity(float targetGravity) {
        float startGravity = currentGravity;
        float timeElapsed;
        Vector3 startColor = new Vector3(playerMaterial.color.r, playerMaterial.color.g, playerMaterial.color.b);
        Vector3 targetColor;
        if (targetGravity == maxGravity) {
            timeElapsed = currentGravity / targetGravity * transitionTime;
            targetColor = heavyColor;
        }
        else {
            //calculates the time but 2x as fast
            timeElapsed = (-((currentGravity - maxGravity) / (minGravity + maxGravity)) * transitionTime) / 2 + transitionTime / 2;
            targetColor = new Vector3(1, 1, 1);
        }

        while (timeElapsed < transitionTime) {
            currentGravity = Mathf.Lerp(startGravity, targetGravity, timeElapsed / transitionTime);
            Vector3 c = Vector3.Lerp(startColor, targetColor, timeElapsed / transitionTime);
            playerMaterial.color = new Color(c.x, c.y, c.z);

            yield return null;
            timeElapsed += Time.deltaTime;
        }
        currentGravity = targetGravity;
        playerMaterial.color = new Color(targetColor.x, targetColor.y, targetColor.z);

    }

    IEnumerator LoadLevel() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }
}
