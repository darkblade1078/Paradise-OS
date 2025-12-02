using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// WYR block ScriptableObject
[CreateAssetMenu(menuName = "Custom/WYRBlock")]
public class WYRBlock : ScriptableObject
{
    [Tooltip("Voice clips for this block")]
    public AudioClip[] preClips;
    [Tooltip("Choices")]
    public WYRChoice[] choices;
    [Tooltip("Display texts for choices")]
    public string blueChoice;
    public string redChoice;
}

[System.Serializable]
public class WYRChoice
{
    public WYRBlock nextBlock;
    public AudioClip[] postClips;
    public int actionObjectIndex = -1;
}