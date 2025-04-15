using UnityEngine;

public class Cell : MonoBehaviour
{
    private void Awake() {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-50,50), Random.Range(-50, 50), Random.Range(-50, 50)));
    }
}
