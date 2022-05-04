using UnityEngine;
using TMPro;
using System.Collections;

public class RaceStart : MonoBehaviour
{
    private TextMeshProUGUI textStart;
    private UIFade uiFade;

    private CarController carController;
    private GameObject[] objectsUI;

    private float countDownTime = 1.0f;
    private int textToInt = 0;

    private void Start()
    {
        textStart = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        uiFade = gameObject.GetComponent<UIFade>();

        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        objectsUI = GameObject.FindGameObjectsWithTag("UI");

        textStart.text = "3";

        carController.enabled = false;

        for (int i = 0; i < objectsUI.Length; i++)
        {
            objectsUI[i].gameObject.SetActive(false);
        }

        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(uiFade.delay);

        while (textStart.text.Equals("1") == false)
        {
            yield return new WaitForSeconds(countDownTime);

            textToInt = int.Parse(textStart.text) - 1;
            textStart.text = textToInt.ToString();
        }

        yield return new WaitForSeconds(countDownTime);

        textStart.text = "GO";

        StartCoroutine(uiFade.FadeDelay(1));

        carController.enabled = true;

        for (int i = 0; i < objectsUI.Length; i++)
        {
            objectsUI[i].gameObject.SetActive(true);
        }

        yield break;
    }
}
