using UnityEngine;
using TMPro;

public class LapDisplay : MonoBehaviour
{
    private TextMeshProUGUI textLapDisplay;

    private void Start()
    {
        textLapDisplay = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void DisplayLaps(int currentLap, int totalLaps)
    {
        textLapDisplay.text = "Lap " + currentLap + "/" + totalLaps;
    }
}
