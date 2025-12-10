using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRHeadsetRemovalHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip removalClip;

    private bool hasTriggered = false;

    private SaveManager saveManager;

    void Start()
    {
        saveManager = SaveManager.instance;
    }

    void OnEnable()
    {
        OVRManager.HMDUnmounted += OnHMDUnmounted;
    }

    void OnDisable()
    {
        OVRManager.HMDUnmounted -= OnHMDUnmounted;
    }

    private void OnHMDUnmounted()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (audioSource != null && removalClip != null)
        {
            saveManager.setupNextScene();
            audioSource.PlayOneShot(removalClip);
            Invoke(nameof(QuitApp), removalClip.length);
        }
        else
        {
            saveManager.setupNextScene();
            QuitApp();
        }
    }

    private void QuitApp()
    {
        Application.Quit();
    }
}