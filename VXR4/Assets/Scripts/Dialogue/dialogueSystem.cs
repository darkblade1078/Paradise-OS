using UnityEngine;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private SaveManager saveManager;

    private void Start()
    {

        saveManager = SaveManager.instance;

        if (audioSource != null && audioClips != null && audioClips.Length > 0)
        {
            StartCoroutine(PlayAudioSequence());
        }
    }

    private IEnumerator PlayAudioSequence()
    {
        foreach (var clip in audioClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        saveManager.GotoNextScene();
    }
}
