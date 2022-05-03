using UnityEngine;
using TMPro;

public class FrameRate : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    private float time = 0.0f;
    private float deltaTime = 0.0f;
    private float pollingTime = 0.1f;
    private int currentFrameRate = 0;

    private void Start()
    {
        fpsText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        CalculateFPS();
    }

    private void CalculateFPS()
    {
        time += Time.deltaTime;
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (time >= pollingTime)
        {
            currentFrameRate = Mathf.RoundToInt(1 / deltaTime);
            time = 0;
        }

        fpsText.text = currentFrameRate.ToString() + " FPS";
    }
}
