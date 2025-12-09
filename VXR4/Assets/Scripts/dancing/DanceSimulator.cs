using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Added for scene management

public class DanceSimulator : MonoBehaviour
{
    [Header("References")]
    public Animator aiAnimator; // Assign AI opponent's Animator
    public PlayerMovementTracker playerMovementTracker; // Assign your movement tracker
    public AudioSource coachAudioSource; // Assign coach AudioSource
    public AudioClip coachTurnClip; // Assign coach turn notification clip
    public AudioClip coachIntroClip; // Assign coach intro line clip
    [Header("Crowd Audio")]
    public AudioSource crowdAudioSource; // Controls crowd sounds
    public AudioClip crowdIdleClip; // Crowd idle loop
    public AudioClip crowdBattleClip; // Crowd battle loop
    public float crowdFadeDuration = 1f; // Duration for crossfade
    [Header("VR Fade")]
    public CanvasGroup vrFadeCanvas; // Assign a CanvasGroup covering the headset view
    [Header("Result Audio")]
    public AudioClip winClip; // Play if player wins
    public AudioClip loseClip; // Play if player loses
    [Header("Dance Music")]
    public AudioSource musicAudioSource; // The single music audio source
    public AudioClip danceSong; // The single song for the whole battle
    [Header("AI Dance Animations")]
    public string[] aiDanceAnimations; // Animation state names for AI
    [Header("Dance Round Timers")]
    public float[] roundTimers; // Duration for each round (seconds)
    private float playerScore = 0f;
    public float aiScore = 5000f;

    void Start()
    {
        if (vrFadeCanvas != null) vrFadeCanvas.alpha = 1f; // Start fully faded
        // Play crowdIdleClip instantly at scene start
        if (crowdAudioSource != null && crowdIdleClip != null)
        {
            crowdAudioSource.clip = crowdIdleClip;
            crowdAudioSource.loop = true;
            crowdAudioSource.Play();
        }
        // Ensure music is stopped at the start
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
        // Fade canvas from black to clear at scene start, then wait 5 seconds, then start coach intro
        StartCoroutine(FadeAndCoachIntroSequence());
    }

    private IEnumerator FadeAndCoachIntroSequence()
    {
        FadeVRCanvas(1f, 0f, crowdFadeDuration);
        yield return new WaitForSeconds(crowdFadeDuration + 5f);
        StartCoroutine(StartWithCoachIntro());
    }

    IEnumerator StartWithCoachIntro()
    {
        PlayCoachIntroClip();
        if (coachAudioSource != null && coachIntroClip != null)
        {
            yield return new WaitForSeconds(coachIntroClip.length);
        }

        PlayCrowdBattleLoop();
        if (musicAudioSource != null && danceSong != null)
        {
            musicAudioSource.clip = danceSong;
            musicAudioSource.loop = false; // Do not loop the music
            musicAudioSource.Play();
        }
        
        StartCoroutine(DanceBattleRoutine());
    }
    void PlayCrowdIdleLoop()
    {
        if (crowdAudioSource != null && crowdIdleClip != null)
        {
            crowdAudioSource.clip = crowdIdleClip;
            crowdAudioSource.loop = true;
            crowdAudioSource.Play();
        }
    }

    void PlayCrowdBattleLoop()
    {
        if (crowdAudioSource != null && crowdBattleClip != null)
        {
            crowdAudioSource.clip = crowdBattleClip;
            crowdAudioSource.loop = true;
            crowdAudioSource.Play();
        }
    }

    void PlayCoachIntroClip()
    {
        if (coachAudioSource != null && coachIntroClip != null)
        {
            coachAudioSource.clip = coachIntroClip;
            coachAudioSource.Play();
        }
    }

    void PlayAIDanceAnimation(int round)
    {
        if (aiAnimator != null && aiDanceAnimations.Length > 0)
        {
            aiAnimator.Play(aiDanceAnimations[round % aiDanceAnimations.Length]);
        }
    }

    void StopAIDanceAnimation()
    {
        if (aiAnimator != null)
        {
            aiAnimator.Play("Idle"); // Replace with your idle animation name
        }
    }

    IEnumerator DanceBattleRoutine()
    {
        int roundIndex = 0;
        while (roundIndex + 1 < roundTimers.Length)
        {
            // AI Turn
            PlayAIDanceAnimation(roundIndex);
            float aiDuration = roundTimers[roundIndex];
            yield return new WaitForSeconds(aiDuration);
            StopAIDanceAnimation();

            // Player Turn
            StopAIDanceAnimation(); // Ensure AI goes idle when player's turn starts
            PlayCoachTurnClip();
            float playerDuration = roundTimers[roundIndex + 1];
            yield return StartCoroutine(TrackPlayerDance(playerDuration));

            roundIndex += 2;
        }
        EndBattle();
    }

    void PlayCoachTurnClip()
    {
        if (coachAudioSource != null && coachTurnClip != null)
        {
            coachAudioSource.clip = coachTurnClip;
            coachAudioSource.Play();
        }
    }

    IEnumerator TrackPlayerDance(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (playerMovementTracker != null)
            {
                if (playerMovementTracker.IsLeftHandMoving) playerScore += Time.deltaTime;
                if (playerMovementTracker.IsRightHandMoving) playerScore += Time.deltaTime;
                if (playerMovementTracker.IsBodyMoving) playerScore += Time.deltaTime;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    void EndBattle()
    {
        PlayCrowdIdleLoop();
        // Play result audio with coachAudioSource
        float resultClipLength = 0f;
        if (playerScore > aiScore)
        {
            if (coachAudioSource != null && winClip != null)
            {
                coachAudioSource.clip = winClip;
                coachAudioSource.loop = false;
                coachAudioSource.Play();
                resultClipLength = winClip.length;
            }
            Debug.Log("Player wins the dance battle!");
        }
        else
        {
            if (coachAudioSource != null && loseClip != null)
            {
                coachAudioSource.clip = loseClip;
                coachAudioSource.loop = false;
                coachAudioSource.Play();
                resultClipLength = loseClip.length;
            }
            Debug.Log("AI wins the dance battle!");
        }
        StartCoroutine(WaitAndFadeOut(resultClipLength));
    }

    private IEnumerator WaitAndFadeOut(float resultClipLength)
    {
        yield return new WaitForSeconds(resultClipLength + 15f);
        FadeVRCanvas(0f, 1f, crowdFadeDuration);
        yield return new WaitForSeconds(crowdFadeDuration);
        // Load next scene by build index
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void FadeVRCanvas(float from, float to, float duration)
    {
        if (vrFadeCanvas != null)
        {
            StartCoroutine(FadeVRCanvasRoutine(from, to, duration));
        }
    }

    private IEnumerator FadeVRCanvasRoutine(float from, float to, float duration)
    {
        if (vrFadeCanvas == null) yield break;
        vrFadeCanvas.alpha = from;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            vrFadeCanvas.alpha = Mathf.Lerp(from, to, progress);
            yield return null;
        }
        vrFadeCanvas.alpha = to;
    }
}
