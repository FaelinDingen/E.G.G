using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private float whiskSpawnTime;
    [SerializeField] private float currentTime;
    [SerializeField] private WhiskSpawner[] whiskSpawners;

    private bool spawnedWhisks;

    private Singleton singleton;

    private void Awake() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
    }

    private void Update() {
        currentTime += Time.deltaTime;
        if (!spawnedWhisks && currentTime > whiskSpawnTime) {
            singleton.whiskPopup.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            foreach (WhiskSpawner whiskSpawner in whiskSpawners) { 
                whiskSpawner.SpawnWhisk();
            }
        }
    }
}
