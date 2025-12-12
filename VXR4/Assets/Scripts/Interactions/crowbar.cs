using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowbar : MonoBehaviour
{
    private bool inHead = false;
    private int hits = 0;
    public AudioSource shatter;
    public GameObject Glass1;
    public GameObject Glass2;
    public GameObject Glass3;
    public GameObject blackScreen;
    public AudioSource voiceclip1;
    public AudioSource voiceclip2;
    public AudioSource voiceclip3;


    private SaveManager saveManager;

    private void Awake()
    {
        saveManager = SaveManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("head"))
        {
            if (!inHead)
            {
                inHead = true;
                breakglass();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("head"))
        {
            inHead = false;
        }
    }

    private void breakglass()
    {
        hits++;

        shatter.Play();

        switch(hits)
        {
            case 1:
                Glass1.SetActive(true);
                voiceclip1.Play();
                break;
            case 2:
                Glass2.SetActive(true);
                voiceclip2.Play();
                break;
            case 3:
                Glass3.SetActive(true);
                StartCoroutine(endScene());
                break;
            case 4:
                StartCoroutine(endSceneNOW());
                break;
            default:
                break;
        }
    }

    IEnumerator endScene()
    {
        voiceclip3.Play();
        yield return new WaitWhile(()=>voiceclip3.isPlaying);
        Glass1.SetActive(false);
        Glass2.SetActive(false);
        Glass3.SetActive(false);
        blackScreen.SetActive(true);
        voiceclip1.Stop();
        voiceclip2.Stop();
        yield return new WaitForSeconds(1f);
        saveManager.GotoNextScene();
    }

    IEnumerator endSceneNOW()
    {
        voiceclip1.Stop();
        voiceclip2.Stop();
        voiceclip3.Stop();
        Glass1.SetActive(false);
        Glass2.SetActive(false);
        Glass3.SetActive(false);
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        saveManager.GotoNextScene();
    }
}
