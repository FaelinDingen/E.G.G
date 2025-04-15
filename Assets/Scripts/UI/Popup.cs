using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private Singleton singleton;
    [SerializeField] private AudioClip errorSound;

    private void OnEnable() {
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        singleton.mainAudioSource.PlayOneShot(errorSound);
    }

    public void RemovePopup() {
        Debug.Log("RemovedPopup");
        gameObject.SetActive(false);
        singleton.WhiskPopupButtons.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        if(EventSystem.current.IsPointerOverGameObject()) {
            Debug.Log("Pointer is over a UI element.");
        }
        else {
            Debug.Log("Touch is NOT over a UI element.");
        }
    }
}
