using UnityEngine;
using TMPro;
using System.Collections;

public class ButtonInteractionManager : MonoBehaviour
{
    [Tooltip("Audio Settings")]
    public AudioSource buttonSource;
    public AudioSource speakerSource;
    public AudioClip buttonPressClip;

    [Tooltip("Text Settings")]
    public string colorName = "";
    public TextMeshProUGUI displayText;

    [Tooltip("Dialogue Settings")]
    public DialogueBlock WYRBlock;
    public DialogueBlock dialogueBlock;
    public DisplayTextBlock displayBlock;

    [Tooltip("Button Settings")]
    public ButtonInteractionManager otherButton;

    // whichever clip is currently playing for our WYR.
    private int currentClip = 0;
    private bool waitingForButton = false;
    private int wyrIndex = 0;

    public void Awake()
    {
        StartCoroutine(WYRLoop());
    }

    public void OnButtonPress()
    {
        if (!waitingForButton) return;
        waitingForButton = false;
        PlayButtonPressSound();
        StartCoroutine(PlayOwnResponse());
    }

    private IEnumerator WYRLoop()
    {
        while (true)
        {
            // Play all clips in current WYRBlock
            if (WYRBlock != null && WYRBlock.voiceClips.Length > 0)
            {
                // Reset both displayTexts to white for new WYR
                displayText.color = Color.white;
                if (otherButton != null && otherButton.displayText != null)
                    otherButton.displayText.color = Color.white;

                for (int i = 0; i < WYRBlock.voiceClips.Length; i++)
                {
                    // Always set text to something valid
                    if (i == 1 && colorName == "Blue" && displayBlock != null)
                    {
                        displayText.text = displayBlock.text;
                    }
                    else if (i == 2 && colorName == "Red" && displayBlock != null)
                    {
                        displayText.text = displayBlock.text;
                    }
                    else if (displayBlock != null)
                    {
                        displayText.text = displayBlock.text;
                    }
                    displayText.enableAutoSizing = true;
                    displayText.ForceMeshUpdate();

                    var clip = WYRBlock.voiceClips[i];
                    speakerSource.Stop();
                    speakerSource.clip = clip;
                    speakerSource.Play();
                    yield return new WaitForSeconds(clip.length);
                }
            }

            // Wait for button press
            waitingForButton = true;
            while (waitingForButton)
            {
                yield return null;
            }

            // Wait for response to finish (handled in PlayResponseAndNextWYR)
            yield return new WaitUntil(() => !speakerSource.isPlaying);

            // Advance to next WYR (if you have multiple blocks, update here)
            wyrIndex++;
        }
    }

    private IEnumerator PlayOwnResponse()
    {
        // Play only this button's assigned dialogueBlock clip and update its text
        if (dialogueBlock != null && dialogueBlock.voiceClips.Length > 0)
        {
            // Play the correct clip and set text for this button
            displayText.text = displayBlock != null ? displayBlock.text : "";
            displayText.color = new Color(1f, 0.84f, 0f); // Gold
            if (otherButton != null && otherButton.displayText != null)
                otherButton.displayText.color = Color.white; // Ensure other stays white
            displayText.enableAutoSizing = true;
            displayText.ForceMeshUpdate();

            // If blue, play first clip; if red, play second clip
            int clipIndex = colorName == "Blue" ? 0 : colorName == "Red" ? 1 : 0;
            if (clipIndex < dialogueBlock.voiceClips.Length)
            {
                var clip = dialogueBlock.voiceClips[clipIndex];
                speakerSource.Stop();
                speakerSource.clip = clip;
                speakerSource.Play();
                yield return new WaitForSeconds(clip.length);
            }
        }
        yield break;
    }

    private void SetTextColor()
    {
        displayText.color = new Color(1f, 0.84f, 0f); // RGB for gold
    }

    private void PlayButtonPressSound()
    {
        if (buttonSource != null && buttonPressClip != null)
        {
            buttonSource.PlayOneShot(buttonPressClip);
        }
    }
}