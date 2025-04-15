using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private bool launchForward = false;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float maxGravity = 20;
    [SerializeField] private float minGravity = 0;
    [SerializeField] private float transitionTime;
    [SerializeField] private Vector3 heavyColor;
    [SerializeField] private GameObject shards;
    [SerializeField] private ParticleSystem windParticleSystem;

    [Header("Audio")]
    [SerializeField] private float grassTarget;
    private float grassTimer;
    [SerializeField] private AudioClip eggCrack;
    [SerializeField] private AudioClip eggCrack2;
    [SerializeField] private AudioClip grass1;
    [SerializeField] private AudioClip grass2;
    [SerializeField] private AudioClip grass3;
    [SerializeField] private AudioSource[] grassSources;
    [SerializeField] private AudioSource windSource;

    private float currentGravity = 0;
    [HideInInspector] public float timer = 0;

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
    private AudioSource audioSource;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        cameraObject = singleton.cameraMovement.gameObject;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        playerMaterial = meshRenderer.material;
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        audioSource = gameObject.GetComponent<AudioSource>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start() {
        if (launchForward) {
            rb.AddForce(Vector3.left * 5000);
        }
    }
    private void Update() {
        timer += Time.deltaTime;
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

        if (Input.GetKeyDown(KeyCode.R) && launchForward) {
            Die();
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
        float playerSpeed = rb.linearVelocity.magnitude;
        Debug.Log(playerSpeed);
        windSource.volume = Mathf.Clamp(playerSpeed / 75, 0.2f, 1f);
        windSource.pitch = Mathf.Clamp(playerSpeed / 75, 0.5f, 2f);

        if (grounded) {
            if (grassTimer > grassTarget) {
                int i = Random.Range(1, 4);
                AudioClip currentClip;
                switch (i) {
                    case 1:
                        currentClip = grass1;
                        break;
                    case 2:
                        currentClip = grass2;
                        break;
                    case 3:
                        currentClip = grass3;
                        break;
                    default:
                        currentClip = grass1;
                        break;
                }
                if (!grassSources[0].isPlaying) {
                    grassSources[0].PlayOneShot(currentClip);
                }
                else if (!grassSources[1].isPlaying) {
                    grassSources[1].PlayOneShot(currentClip);
                }
                else if (!grassSources[2].isPlaying) {
                    grassSources[2].PlayOneShot(currentClip);
                }
                grassTimer = 0;
                grassSources[0].volume = Mathf.Clamp(playerSpeed / 75, 0.2f, 1f);
                grassSources[0].pitch = Mathf.Clamp(playerSpeed / 75, 0.5f, 2f);
                grassSources[1].volume = Mathf.Clamp(playerSpeed / 75, 0.2f, 1f);
                grassSources[1].pitch = Mathf.Clamp(playerSpeed / 75, 0.5f, 2f);
                grassSources[2].volume = Mathf.Clamp(playerSpeed / 75, 0.2f, 1f);
                grassSources[2].pitch = Mathf.Clamp(playerSpeed / 75, 0.5f, 2f);
            }
            else {
                grassTimer += playerSpeed * Time.deltaTime;
            }
        }

        if (playerSpeed > 30) {
            var emission = windParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(0, 50, playerSpeed / 75);
            var main = windParticleSystem.main;
            main.startSpeed = Mathf.Lerp(.3f, 20, playerSpeed / 75);
        }
        else {
            var emission = windParticleSystem.emission;
            emission.rateOverTime = 0;
        }
    }

    private void FixedUpdate() {
        if (grounded) {
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, groundCheckDistance)) {
                groundNormal = hit.normal;
            }
        }
        else {
            groundNormal = Vector3.zero;
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

        rb.AddForce(projectedMovement * speed);

        Vector3 customGravity = new Vector3(0, -currentGravity, 0);
        Vector3 projectedGravity = Vector3.ProjectOnPlane(customGravity, groundNormal);
        Debug.DrawRay(transform.position, projectedGravity * 5, Color.red, 25);
        rb.AddForce(projectedGravity, ForceMode.Acceleration);
    }

    public void Die() {
        meshRenderer.enabled = false;
        sphereCollider.enabled = false;
        rb.useGravity = false;

        shards.SetActive(true);
        audioSource.PlayOneShot(Random.Range(0, 2) == 1 ? eggCrack : eggCrack2);
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
        SceneManager.LoadScene(2);
    }
}
