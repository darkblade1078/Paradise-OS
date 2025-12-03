using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float holdTime = .2f;
    [SerializeField] private float fadeTime = 3f;

    void Start()
    {
        StartCoroutine(FlashThenFade());
    }

    IEnumerator FlashThenFade()
    {
        // Short optional delay before fading (can remove)
        yield return new WaitForSeconds(holdTime);

        // Fade from white to transparent
        yield return StartCoroutine(FadeOut(image, fadeTime));
    }

    IEnumerator FadeOut(Image image, float timeToFade)
    {
        Color start = image.color; // should be white now
        Color end = new Color(start.r, start.g, start.b, 0f);

        float t = 0f;

        while (t < timeToFade)
        {
            t += Time.deltaTime;
            image.color = Color.Lerp(start, end, t / timeToFade);
            yield return null;
        }

        image.color = end;
    }
}