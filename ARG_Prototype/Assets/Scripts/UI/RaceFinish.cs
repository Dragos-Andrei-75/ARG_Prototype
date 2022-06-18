using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class RaceFinish : MonoBehaviour
{
    private TextMeshProUGUI textRecordedTime;
    private UIFade uiFade;

    private Menu menu;

    private CarController carController;
    private GameObject[] objectsUI;

    private void Start()
    {
        textRecordedTime = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        uiFade = gameObject.GetComponent<UIFade>();

        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        objectsUI = GameObject.FindGameObjectsWithTag("UI");

        menu = GameObject.Find("Menu").GetComponent<Menu>();

        for (int i = 0; i < objectsUI.Length - 2; i++)
        {
            objectsUI[i] = objectsUI[i + 2];
        }

        Array.Resize(ref objectsUI, objectsUI.Length - 2);
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
        textRecordedTime.text = "Finish\n" + Clock.textClock.text;

        carController.enabled = false;

        for (int i = 0; i < objectsUI.Length; i++)
        {
            objectsUI[i].gameObject.SetActive(false);
        }

        StartCoroutine(uiFade.FadeDelay(1));
        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5);

        menu.QuitGame();
    }
}
