using System.Collections;
using System.Collections.Generic;
using Ionic.Zip;
using UnityEngine;

public class curtainRoll : MonoBehaviour
{   
    public Light spotlight;
    public Light roomlight;
    public GameObject curtain;
    public GameObject Flashbang;
    public GameObject Instruments;
    public AudioSource curtainRise;
    public AudioSource SpotLightSFX;
    public float targetHeight = 5f; 
    public float duration = 34f;

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0, targetHeight, 0);

        StartCoroutine(RiseCurtain());
    }

    IEnumerator RiseCurtain()
    {
        yield return new WaitForSeconds(3f);

        StartCoroutine(LightsAndSfx());

        Vector3 startPos = curtain.transform.position;
        Vector3 endPos = startPos + new Vector3(0, targetHeight, 0);

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            curtain.transform.position = Vector3.Lerp(startPos, endPos, progress);

            yield return null;
        }

        curtain.transform.position = endPos;
    }

    IEnumerator LightsAndSfx()
    {
        curtainRise.Play();
        yield return new WaitWhile(()=>curtainRise.isPlaying);

        SpotLightSFX.Play();
        spotlight.enabled=true;

        yield return new WaitForSeconds(.1f);

        Flashbang.SetActive(true);

        yield return new WaitForSeconds(1f);

        Instruments.SetActive(true);
    }
}
