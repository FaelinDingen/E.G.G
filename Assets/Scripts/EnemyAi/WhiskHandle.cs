using UnityEngine;

public class WhiskHandle : MonoBehaviour
{
    [SerializeField] private Whisk whisk;

    private void OnTriggerEnter(Collider other) {
        whisk.OnTriggerEnter(other);
    }
}
