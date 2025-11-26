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

    [Header("WYR Dialogue Block")]
    public DialogueBlock WYRBlock;

    private bool introPlayed = false;
    private bool waitingForButton = false;
    private int wyrIndex = 0;

    private void Awake()
    {
        // Play intro dialogue first, then wait for button press to start WYR block
        if (introBlock != null && introBlock.voiceClips != null && introBlock.voiceClips.Length > 0 && !introPlayed)
        {
            StartCoroutine(PlayIntroCoroutine());
        }
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
        // After intro, automatically play first WYR block
        StartCoroutine(PlayWYRBlockClipsCoroutine());
    }

    private IEnumerator PlayWYRBlockClipsCoroutine()
    {
        // Reset both buttons' text to white at the start of WYR block
        if (redButton != null && redButton.displayText != null)
            redButton.displayText.color = Color.white;
        if (blueButton != null && blueButton.displayText != null)
            blueButton.displayText.color = Color.white;

        for (int i = 0; i < WYRBlock.voiceClips.Length; i++)
        {
            speakerSource.Stop();
            speakerSource.clip = WYRBlock.voiceClips[i];
            speakerSource.Play();
            yield return new WaitWhile(() => speakerSource.isPlaying);
        }
        // After WYR block, wait for button press
        waitingForButton = true;
    }

    public void PlayButtonDialogue(ButtonInteraction button)
    {
        // Only respond to button press if waitingForButton is true
        if (!waitingForButton)
            return;
        waitingForButton = false;

        // Update the displayText with the DisplayTextBlock's text and set color to gold
        if (button.displayBlock != null && button.displayText != null)
        {
            button.displayText.text = button.displayBlock.text;
            button.displayText.color = new Color(1f, 0.84f, 0f); // Gold
        }

        // Advance to next blocks before playing dialogue
        AdvanceToNextBlocks();

        // Start coroutine to play all clips in sequence, then play next WYR block
        StartCoroutine(PlayButtonResponseAndNextWYR(button));
    }

    private IEnumerator PlayButtonResponseAndNextWYR(ButtonInteraction button)
    {
        yield return StartCoroutine(PlayDialogueClipsCoroutine(button));
        // After response, play next WYR block
        StartCoroutine(PlayWYRBlockClipsCoroutine());
    }

    private IEnumerator PlayDialogueClipsCoroutine(ButtonInteraction button)
    {
        var clips = button.dialogueBlock.voiceClips;
        if (clips != null && clips.Length > 0 && speakerSource != null)
        {
            for (int i = 0; i < clips.Length; i++)
            {
                speakerSource.Stop();
                speakerSource.clip = clips[i];
                speakerSource.Play();

                // Wait until the clip is done playing
                yield return new WaitWhile(() => speakerSource.isPlaying);

                // For the second clip (i == 1), update blue button displayText
                if (i == 1 && blueButton != null && blueButton.displayText != null && blueButton.displayBlock != null)
                {
                    blueButton.displayText.text = blueButton.displayBlock.text;
                }
                // For the third clip (i == 2), update red button displayText
                if (i == 2 && redButton != null && redButton.displayText != null && redButton.displayBlock != null)
                {
                    redButton.displayText.text = redButton.displayBlock.text;
                }
            }
            // After final clip, advance to next WYRBlock and update buttons' display blocks
            AdvanceToNextBlocks();
        }
    }

    private void AdvanceToNextBlocks()
    {
        // Advance WYRBlock if it has a nextBlock property
        if (WYRBlock != null && WYRBlock.defaultBlock != null)
        {
            WYRBlock = WYRBlock.defaultBlock;
        }
        // Advance blue button's display block if it has nextBlock
        if (blueButton != null && blueButton.displayBlock != null && blueButton.displayBlock.nextBlock != null)
        {
            blueButton.displayBlock = blueButton.displayBlock.nextBlock;
        }
        // Advance red button's display block if it has nextBlock
        if (redButton != null && redButton.displayBlock != null && redButton.displayBlock.nextBlock != null)
        {
            redButton.displayBlock = redButton.displayBlock.nextBlock;
        }
    }
}