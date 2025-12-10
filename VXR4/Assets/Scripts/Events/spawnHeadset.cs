using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnHeadset : MonoBehaviour
{
    public Renderer targetRenderer;
    public float fadeDuration = 2f;
    private Material targetMaterial;

    void Start()
    {
        if (targetRenderer != null)
        {
            targetMaterial = targetRenderer.material;

            StartCoroutine(FadeDissolveCoroutine());      
        }
    }

    private IEnumerator FadeDissolveCoroutine()
    {
        float startValue = 1f;
        float endValue = 0f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsed / fadeDuration);
            targetMaterial.SetFloat("_DissolveAmount", newValue);
            elapsed += Time.deltaTime;
            yield return null;
        }
        targetMaterial.SetFloat("_DissolveAmount", endValue);
    }
}