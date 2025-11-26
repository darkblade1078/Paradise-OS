using UnityEngine;
using System.Collections;

// Dialogue block ScriptableObject
[CreateAssetMenu(menuName = "Dialogue/DialogueBlock")]
public class DialogueBlock : ScriptableObject
{
    [Tooltip("Voice clips for this block")]
    public AudioClip[] voiceClips;
    public DialogueChoice[] choices;
    [Tooltip("Use default block instead of choices")]
    public bool useDefaultBlock;
    [Tooltip("Default block to go to if useDefaultBlock is true")]
    public DialogueBlock defaultBlock;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueBlock nextBlock;
}