using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private CarController carController;
    private UIFade uiFade;

    private void Start()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        uiFade = gameObject.GetComponent<UIFade>();
    }

    private void OnEnable()
    {
        CheckPointHandler.OnFinish += FinishRace;
    }

    private void OnDisable()
    {
        CheckPointHandler.OnFinish -= FinishRace;
    }

    private void FinishRace()
    {
        carController.enabled = false;

        StartCoroutine(uiFade.FadeDelay(1));
    }
}
