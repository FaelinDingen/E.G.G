using UnityEngine;

public class WhiskSpawner : MonoBehaviour
{
    [SerializeField] private GameObject whisk;
    public void SpawnWhisk() { 
        Instantiate(whisk, transform.position, transform.rotation);
    }
}
