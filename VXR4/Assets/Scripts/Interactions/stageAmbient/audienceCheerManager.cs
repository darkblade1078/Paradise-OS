using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audienceCheerManager : MonoBehaviour
{
    public AudioSource cheer1;
    public AudioSource cheer2;
    public AudioSource cheer3;
    public AudioSource boo;

    public void StartCheer()
    {
        StopAllCoroutines();
        StartCoroutine(AudCheer());
    }

    
    IEnumerator AudCheer()
    {
        yield return new WaitForSeconds(5f);
        int randNum = Random.Range(1, 11);

        switch(randNum)
        {
            case <= 3:
                cheer1.Play();
                break;

            case <= 6:
                cheer2.Play();
                break;

            case <= 9:
                cheer3.Play();
                break;

            case 10:
                boo.Play();
                break;
        }
    }
}
