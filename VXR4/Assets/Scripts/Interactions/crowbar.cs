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
    public AudioSource voiceclip1;
    public AudioSource voiceclip2;
    public AudioSource voiceclip3;
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
        }
    }

    IEnumerator endScene()
    {
        voiceclip3.Play();
        yield return new WaitForSeconds(5f);
        //jump cut to black here
        //scene change to evil butler here
    }
}
