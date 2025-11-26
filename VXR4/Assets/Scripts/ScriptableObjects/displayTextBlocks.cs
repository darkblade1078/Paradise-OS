using UnityEngine;
using System.Collections;

// DisplayTextBlock block ScriptableObject
[CreateAssetMenu(menuName = "Custom/DisplayTextBlock")]
public class DisplayTextBlock : ScriptableObject
{
    [Tooltip("The text to display")]
    public string text;

    [Tooltip("Next Block")]
    public DisplayTextBlock nextBlock;
}