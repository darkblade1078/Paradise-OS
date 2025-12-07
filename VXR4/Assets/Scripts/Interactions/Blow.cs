using System.Collections;
using UnityEngine;

public class Blowing : MonoBehaviour
{
    public bool blown = false;
    public AudioSource note1;
    public AudioSource note2;
    public AudioSource note3;
    public AudioSource note4;
    public AudioSource note5;
    public AudioSource note6;

     public audienceCheerManager cheerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("head"))
        {
            StopAllCoroutines();
            
            StartCoroutine(Saxophone());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("head"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Saxophone()
    {
        while(true)
        {
            int randNum = Random.Range(1, 7);

            switch(randNum)
            {
                case 1:
                    note1.Play();
                    break;

                case 2:
                    note2.Play();
                    break;

                case 3:
                    note3.Play();
                    break;

                case 4:
                    note4.Play();
                    break;

                case 5:
                    note5.Play();
                    break;

                case 6:
                    note6.Play();
                    break;
            }
            cheerManager.StartCheer();
            yield return new WaitForSeconds(3f);
        }
    }
}