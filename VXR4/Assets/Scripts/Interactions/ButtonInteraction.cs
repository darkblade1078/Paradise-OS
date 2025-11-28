using UnityEngine;
using TMPro;

public class ButtonInteraction : MonoBehaviour
{
    [Header("General Settings")]
    public ButtonInteractionManager interactionManager;

    [Header("Audio Settings")]
    public AudioSource buttonSource;
    public AudioClip buttonClip;

    [Header("Button Text")]
    public TextMeshProUGUI buttonText;

    public void OnButtonPress()
    {
        if (buttonSource != null && buttonClip != null)
        {
            buttonSource.PlayOneShot(buttonClip);
        }
        // Notify the manager to play this button's dialogue
        if (interactionManager != null)
        {
            interactionManager.PlayButtonDialogue(this);
        }
    }
}