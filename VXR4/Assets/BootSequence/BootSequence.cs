using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootAnimation : MonoBehaviour
{
    private Animator anim;
    private AudioSource sound;
    private SaveManager saveManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        saveManager = SaveManager.instance;
        StartCoroutine(bootPlay());
    }

    IEnumerator bootPlay()
    {
        sound.Play();
        yield return new WaitForSeconds(7.1f);
        anim.Play("bootAnim");  
        yield return new WaitWhile(()=>sound.isPlaying);

        if (saveManager != null)
            saveManager.GoToNextScene();
    }
}
