using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audienceNoises : MonoBehaviour
{
    public AudioSource cough;
    public AudioSource ping;
    public AudioSource squeak;
    public AudioSource murmur;

    void Start()
    {
        StartCoroutine(AudSounds());
    }

    
    IEnumerator AudSounds()
    {
        while(true)
        {
            int secs = Random.Range(20, 45);
            yield return new WaitForSeconds(secs);
            int randNum = Random.Range(1, 5);

            switch(randNum)
            {
                case 1:
                    cough.Play();
                    break;

                case 2:
                    ping.Play();
                    break;

                case 3:
                    squeak.Play();
                    break;

                case 4:
                    murmur.Play();
                    break;
            }

        }
    }
}
