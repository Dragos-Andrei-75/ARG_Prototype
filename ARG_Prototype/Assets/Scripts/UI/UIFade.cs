using UnityEngine;
using System.Collections;

public class UIFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private Pause pause;

    public float delay = 0.0f;
    [SerializeField] private float delta = 0.0f;
    [SerializeField] private bool initialFade = false;
    [SerializeField] private bool gameplayUI = false;

    private void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        if (gameplayUI == true) pause = GameObject.Find("Menu").GetComponent<Pause>();

        delta = Time.fixedDeltaTime * 10;
    }

    private void OnEnable()
    {
        if (initialFade == true) StartCoroutine(FadeDelay(delay));
        if (gameplayUI == true) Pause.onPause += HandleUI;
    }

    private void OnDisable()
    {
        if (gameplayUI == true) Pause.onPause -= HandleUI;
    }

    private void HandleUI()
    {
        if (pause != null) pause.InputDisable();

        if (canvasGroup.alpha == 1) StartCoroutine(FadeOut());
        else StartCoroutine(FadeIn());
    }

    public IEnumerator FadeDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        HandleUI();

        yield break;
    }

    private IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += delta;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        canvasGroup.alpha = 1;

        if (pause != null) pause.InputEnable();

        yield break;
    }

    private IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= delta;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        canvasGroup.alpha = 0;

        if (pause != null) pause.InputEnable();

        yield break;
    }
}
