using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;
using TMPro;

public class QuestVoiceDialogue : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Wit wit;
    
    [Header("UI (Optional)")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI transcriptionText;
    [SerializeField] private GameObject listeningIndicator;
    
    private bool isListening = false;

    void Start()
    {
        // Get Wit component if not assigned
        if (wit == null)
        {
            wit = GetComponent<Wit>();
        }
        
        if (wit == null)
        {
            Debug.LogError("Wit component not found!");
            return;
        }
        
        // Subscribe to events
        wit.VoiceEvents.OnStartListening.AddListener(OnStartListening);
        wit.VoiceEvents.OnStoppedListening.AddListener(OnStoppedListening);
        wit.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        wit.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);
        wit.VoiceEvents.OnError.AddListener(OnError);
        
        UpdateStatus("Ready! Press A to talk.");
        
        if (listeningIndicator != null)
        {
            listeningIndicator.SetActive(false);
        }
    }

    void Update()
    {
        // Press A button on right controller to activate voice
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            ActivateVoice();
        }
        
        // For testing in Unity Editor
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateVoice();
        }
    }

    void ActivateVoice()
    {
        if (wit != null && !wit.Active)
        {
            wit.Activate();
            Debug.Log("Voice activated - speak now!");
        }
    }

    void DeactivateVoice()
    {
        if (wit != null && wit.Active)
        {
            wit.Deactivate();
        }
    }

    // Called when Wit starts listening
    void OnStartListening()
    {
        isListening = true;
        Debug.Log("🎤 Listening...");
        UpdateStatus("🎤 Listening...");
        
        if (listeningIndicator != null)
        {
            listeningIndicator.SetActive(true);
        }
    }

    // Called when Wit stops listening
    void OnStoppedListening()
    {
        isListening = false;
        Debug.Log("Processing...");
        UpdateStatus("Processing...");
        
        if (listeningIndicator != null)
        {
            listeningIndicator.SetActive(false);
        }
    }

    // Called while user is speaking (real-time)
    void OnPartialTranscription(string transcription)
    {
        Debug.Log("Partial: " + transcription);
        UpdateTranscription(transcription + "...");
    }

    // Called when transcription is complete
    void OnFullTranscription(string transcription)
    {
        Debug.Log("✅ Full transcription: " + transcription);
        UpdateTranscription(transcription);
        UpdateStatus("Ready! Press A to talk.");
        
        // Process the dialogue choice
        ProcessDialogueInput(transcription);
    }

    // Called if there's an error
    void OnError(string error, string message)
    {
        Debug.LogError($"Wit Error: {error} - {message}");
        UpdateStatus("Error: " + message);
        UpdateTranscription("Could not understand. Try again.");
        
        if (listeningIndicator != null)
        {
            listeningIndicator.SetActive(false);
        }
    }

    // Process what the user said
    void ProcessDialogueInput(string userSpeech)
    {
        string lower = userSpeech.ToLower();
        
        Debug.Log($"Processing: '{userSpeech}'");
        
        // Simple keyword matching
        if (lower.Contains("yes") || lower.Contains("yeah") || lower.Contains("sure") || lower.Contains("yep"))
        {
            OnDialogueChoice("yes");
        }
        else if (lower.Contains("no") || lower.Contains("nope") || lower.Contains("nah"))
        {
            OnDialogueChoice("no");
        }
        else if (lower.Contains("maybe") || lower.Contains("not sure") || lower.Contains("i don't know"))
        {
            OnDialogueChoice("maybe");
        }
        else
        {
            OnDialogueChoice("unknown");
        }
    }

    // Handle the dialogue choice
    void OnDialogueChoice(string choice)
    {
        Debug.Log($"💬 Dialogue choice: {choice.ToUpper()}");
        
        // TODO: Connect this to your dialogue system
        // Example: DialogueManager.Instance.ProcessChoice(choice);
        
        switch (choice)
        {
            case "yes":
                Debug.Log("User agreed!");
                // Your code for YES option
                break;
                
            case "no":
                Debug.Log("User disagreed!");
                // Your code for NO option
                break;
                
            case "maybe":
                Debug.Log("User is uncertain!");
                // Your code for MAYBE option
                break;
                
            default:
                Debug.Log("Didn't understand the choice");
                break;
        }
    }

    // Helper methods for UI updates
    void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = status;
        }
    }

    void UpdateTranscription(string text)
    {
        if (transcriptionText != null)
        {
            transcriptionText.text = text;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (wit != null && wit.VoiceEvents != null)
        {
            wit.VoiceEvents.OnStartListening.RemoveListener(OnStartListening);
            wit.VoiceEvents.OnStoppedListening.RemoveListener(OnStoppedListening);
            wit.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
            wit.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
            wit.VoiceEvents.OnError.RemoveListener(OnError);
        }
    }
}