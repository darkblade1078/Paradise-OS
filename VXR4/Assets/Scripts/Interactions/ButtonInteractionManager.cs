using UnityEngine;
using TMPro;
using System.Collections;

public class ButtonInteractionManager : MonoBehaviour
{
    [Header("Buttons")]
    public ButtonInteraction redButton;
    public ButtonInteraction blueButton;

    [Header("Speaker Audio Source")]
    public AudioSource speakerSource;

    [Header("Intro Dialogue")]
    public DialogueBlock introBlock;

    [Header("WYR Block")]
    public WYRBlock wYRBlock;

    private bool introPlayed = false;
    private bool waitingForButton = false;
    private ButtonInteraction lastButtonPressed;

    private void Awake()
    {
        // Play intro dialogue first, then wait for button press to start WYR block
        if (introBlock != null && introBlock.voiceClips != null && introBlock.voiceClips.Length > 0 && !introPlayed)
            StartCoroutine(PlayIntroCoroutine());
    }

    private IEnumerator PlayIntroCoroutine()
    {
        introPlayed = true;
        for (int i = 0; i < introBlock.voiceClips.Length; i++)
        {
            speakerSource.Stop();
            speakerSource.clip = introBlock.voiceClips[i];
            speakerSource.Play();
            yield return new WaitWhile(() => speakerSource.isPlaying);
        }

        // After intro, set button texts to first WYR block's choices
        if (wYRBlock != null)
        {
            if (blueButton != null && blueButton.buttonText != null)
                blueButton.buttonText.text = wYRBlock.blueChoice;
            if (redButton != null && redButton.buttonText != null)
                redButton.buttonText.text = wYRBlock.redChoice;
        }

        StartCoroutine(PlayWYRBlockClipsCoroutine());
    }

    private IEnumerator PlayWYRBlockClipsCoroutine()
    {
        // Reset both buttons' text color to white at the start of WYR block
        if (redButton != null && redButton.buttonText != null)
            redButton.buttonText.color = Color.white;
        if (blueButton != null && blueButton.buttonText != null)
            blueButton.buttonText.color = Color.white;
        // Set button texts to current block's choices at the start
        if (wYRBlock != null)
        {
            if (blueButton != null && blueButton.buttonText != null)
                blueButton.buttonText.text = wYRBlock.blueChoice;
            if (redButton != null && redButton.buttonText != null)
                redButton.buttonText.text = wYRBlock.redChoice;
        }
        // Play preClips before button press
        if (wYRBlock != null && wYRBlock.preClips != null)
        {
            for (int i = 0; i < wYRBlock.preClips.Length; i++)
            {
                speakerSource.Stop();
                speakerSource.clip = wYRBlock.preClips[i];
                speakerSource.Play();
                yield return new WaitWhile(() => speakerSource.isPlaying);
            }
        }
        waitingForButton = true;
    }

    public void PlayButtonDialogue(ButtonInteraction button)
    {
        if (!waitingForButton)
            return;

        waitingForButton = false;
        lastButtonPressed = button;

        // Stop any currently playing audio before advancing
        if (speakerSource != null && speakerSource.isPlaying)
            speakerSource.Stop();

        // Set button text to gold when pressed
        if (button != null && button.buttonText != null)
            button.buttonText.color = new Color(1f, 0.84f, 0f); // Gold
        AdvanceToNextBlocks();
        StartCoroutine(PlayButtonResponseAndNextWYR(button));
    }

    private IEnumerator PlayButtonResponseAndNextWYR(ButtonInteraction button)
    {
        // Play postClips from the chosen WYRChoice before branching
        int choiceIndex = (button == blueButton) ? 0 : (button == redButton) ? 1 : -1;
        if (wYRBlock != null && wYRBlock.choices != null && choiceIndex >= 0 && choiceIndex < wYRBlock.choices.Length)
        {
            var choice = wYRBlock.choices[choiceIndex];
            if (choice.postClips != null)
            {
                for (int i = 0; i < choice.postClips.Length; i++)
                {
                    speakerSource.Stop();
                    speakerSource.clip = choice.postClips[i];
                    speakerSource.Play();
                    yield return new WaitWhile(() => speakerSource.isPlaying);
                }
            }

            // Run UnityEvent for this choice only
            if (choice.onChosen != null)
                choice.onChosen.Invoke();

            // Branch to next block if present
            if (choice.nextBlock != null)
                wYRBlock = choice.nextBlock;
            else
            {
                // End of script: disable buttons and stop
                if (blueButton != null) blueButton.gameObject.SetActive(false);
                if (redButton != null) redButton.gameObject.SetActive(false);
                yield break;
            }
        }

        // Update button texts to new WYR block's choices
        if (wYRBlock != null)
        {
            if (blueButton != null && blueButton.buttonText != null)
                blueButton.buttonText.text = wYRBlock.blueChoice;
            if (redButton != null && redButton.buttonText != null)
                redButton.buttonText.text = wYRBlock.redChoice;
        }

        // Play next WYR block's preClips
        StartCoroutine(PlayWYRBlockClipsCoroutine());
    }

    private void AdvanceToNextBlocks()
    {
        // Branching logic for WYRBlock
        if (wYRBlock != null && wYRBlock.choices != null && wYRBlock.choices.Length > 0)
        {
            int choiceIndex = (lastButtonPressed == blueButton) ? 0 : (lastButtonPressed == redButton) ? 1 : -1;
            if (choiceIndex >= 0 && choiceIndex < wYRBlock.choices.Length)
            {
                // Run UnityEvent for this choice only
                if (wYRBlock.choices[choiceIndex].onChosen != null)
                    wYRBlock.choices[choiceIndex].onChosen.Invoke();
                if (wYRBlock.choices[choiceIndex].nextBlock != null)
                    wYRBlock = wYRBlock.choices[choiceIndex].nextBlock;
            }
        }
        // Update button texts to new WYR block's choices
        if (wYRBlock != null)
        {
            if (blueButton != null && blueButton.buttonText != null)
                blueButton.buttonText.text = wYRBlock.blueChoice;

            if (redButton != null && redButton.buttonText != null)
                redButton.buttonText.text = wYRBlock.redChoice;
        }
    }
}