using System.Collections;
using UnityEngine;

public class WhiskSpawnerRepeater : MonoBehaviour
{
    [SerializeField] private GameObject whisk;

    private void Start() {
        StartCoroutine(spawnWhisk());
    }

    IEnumerator spawnWhisk() {
        Destroy(Instantiate(whisk, transform.position, transform.rotation), 30);
        yield return new WaitForSeconds(Random.Range(3,10));
        StartCoroutine(spawnWhisk());
    }
}
