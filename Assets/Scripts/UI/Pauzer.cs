using UnityEngine;

public class Pauzer : MonoBehaviour
{
    private Singleton Singleton;
    private bool pauzed = false;

    private void Awake() {
        Singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            togglePauze();
        }
    }

    public void togglePauze() {
        pauzed = !pauzed;
        Time.timeScale = pauzed ? 0 : 1;
        Singleton.pauseScreen.SetActive(pauzed);
        Cursor.lockState = pauzed ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pauzed;
    }
}
