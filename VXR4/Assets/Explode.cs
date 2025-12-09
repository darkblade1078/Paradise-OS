using UnityEngine;
using UnityEngine.Video;

public class Explode : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource explosionSound;
    public GameObject explosionEffect;   // particle system or light
    public GameObject uiCanvas;          // your UI canvas to disable
    public GameObject videoScreen;       // the screen or object holding the video

    private void Start()
    {
        // Subscribe to event when video finishes
        videoPlayer.loopPointReached += OnVideoEnd;

        // Ensure explosion effect starts off
        if (explosionEffect != null)
            explosionEffect.SetActive(false);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Activate explosion VFX
        if (explosionEffect != null)
            explosionEffect.SetActive(true);

        // Play explosion audio
        if (explosionSound != null)
            explosionSound.Play();

        // Disable UI canvas
        if (uiCanvas != null)
            uiCanvas.SetActive(false);

        // Disable the video screen
        if (videoScreen != null)
            videoScreen.SetActive(false);
    }
}
