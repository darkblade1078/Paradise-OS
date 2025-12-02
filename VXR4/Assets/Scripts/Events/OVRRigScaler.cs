using UnityEngine;
using System.Collections;

public class OVRRigScaler : MonoBehaviour
{
    public Transform ovrRig; 
    public AudioSource audioSource; 
    public AudioClip postScaleClip; 
    public float scaleMultiplier = 2f;  
    public float growDelay = 0.5f; 

    private Vector3 originalScale;

    private void Awake()
    {
        if (ovrRig != null)
            originalScale = ovrRig.transform.localScale;
    }

    public void Execute()
    {
        StartCoroutine(ScaleRoutine());
    }

    private IEnumerator ScaleRoutine()
    {
        if (ovrRig == null || audioSource == null) yield break;

        // Wait while audioSource is playing (initial dialogue)
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Optional delay before scaling
        if (growDelay > 0)
            yield return new WaitForSeconds(growDelay);

        // Scale
        ovrRig.transform.localScale = originalScale * scaleMultiplier;

        // Play post scale clip
        if (postScaleClip != null)
        {
            audioSource.clip = postScaleClip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // Optional delay before scaling back
        if (growDelay > 0)
            yield return new WaitForSeconds(growDelay);

        // Scale back to normal
        ovrRig.transform.localScale = originalScale;
    }
}