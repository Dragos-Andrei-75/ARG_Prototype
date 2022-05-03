using UnityEngine;
using TMPro;
using System.Collections;

public class CountDownStart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textStart;
    [SerializeField] private UIFade uiFade;

    private float countDownTime = 1.0f;
    private int textToInt = 0;

    private void Start()
    {
        textStart = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        uiFade = gameObject.GetComponent<UIFade>();

        textStart.text = "3";

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

        yield break;
    }
}
