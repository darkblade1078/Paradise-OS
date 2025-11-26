using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystemManager : MonoBehaviour
{
    [Header("Dialogue System References")]
    public DialogueBlock currentBlock;
    public AudioSource audioSource;
    public GameObject choiceUI;

    private Coroutine dialogueCoroutine;

    void Start()
    {
        if (currentBlock != null)
            PlayDialogueBlock(currentBlock);
    }

    public void PlayDialogueBlock(DialogueBlock block)
    {
        currentBlock = block;
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(PlayBlockCoroutine());
    }

    IEnumerator PlayBlockCoroutine()
    {
        int clipCount = currentBlock.voiceClips != null ? currentBlock.voiceClips.Length : 0;
        for (int i = 0; i < clipCount; i++)
        {
            var clip = currentBlock.voiceClips[i];
            if (clip == null) continue;
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
        }
            
        // Decision logic for next block
        if (currentBlock.useDefaultBlock && currentBlock.defaultBlock != null)
            PlayDialogueBlock(currentBlock.defaultBlock);
        else if (currentBlock.choices != null && currentBlock.choices.Length > 0)
            ShowChoiceUI(currentBlock.choices);
        else
            EndDialogue();
    }

    void ShowChoiceUI(DialogueChoice[] choices)
    {
        if (choiceUI != null)
            choiceUI.SetActive(true);

        // TODO: Implement your own UI logic to show choices from the block
        StartCoroutine(PickChoiceByVoice(choices));
    }

    IEnumerator PickChoiceByVoice(DialogueChoice[] choices)
    {
        // TODO: Integrate Meta SDK microphone and voice recognition
        yield return new WaitForSeconds(2f); // Simulate waiting for user response

        int pickedIndex = 0; // Dummy: always pick first choice

        if (choiceUI != null)
            choiceUI.SetActive(false);
        if (choices != null && choices.Length > 0 && choices[pickedIndex] != null && choices[pickedIndex].nextBlock != null)
            PlayDialogueBlock(choices[pickedIndex].nextBlock);
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        if (choiceUI != null)
            choiceUI.SetActive(false);
    }
}
