using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time;
    private void Awake() {
        time.text = DataCarrier.finishedTime.ToString();
    }
}
