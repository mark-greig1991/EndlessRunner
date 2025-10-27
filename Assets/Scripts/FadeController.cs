using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private Image fadeImage;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeIn(float duration)
    {
        gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            fadeImage.color = color;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        gameObject.SetActive(false);
    }

    public IEnumerator FadeOut(float duration)
    {
        gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            fadeImage.color = color;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
