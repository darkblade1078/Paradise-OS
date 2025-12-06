using UnityEngine;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private OVRVirtualKeyboard keyboard;
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        if (keyboard != null)
        {
            // Subscribe to keyboard events
            keyboard.CommitTextEvent.AddListener(OnTextCommit);
            keyboard.BackspaceEvent.AddListener(OnBackspace);
            keyboard.KeyboardHiddenEvent.AddListener(OnKeyboardHidden);
            
            // Start hidden
            keyboard.gameObject.SetActive(false);
        }

        if (inputField != null)
        {
            inputField.onSelect.AddListener(OnInputSelected);
        }

        // Configure interaction method
        ConfigureInteractionMethod();
    }

    private void ConfigureInteractionMethod()
    {
        if (keyboard == null)
            return;
    }

    private void OnTextCommit(string text)
    {
        if (inputField != null)
        {
            inputField.text += text;
        }
    }

    private void OnBackspace()
    {
        if (inputField != null && inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    private void OnInputSelected(string text)
    {
        if (keyboard != null)
        {
            keyboard.gameObject.SetActive(true);
        }
    }

    private void OnKeyboardHidden()
    {
        if (keyboard != null)
        {
            keyboard.gameObject.SetActive(false);
        }
    }

    // Call this method to manually show/hide keyboard
    public void ToggleKeyboard()
    {
        if (keyboard != null)
            keyboard.gameObject.SetActive(!keyboard.gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        if (keyboard != null)
        {
            keyboard.CommitTextEvent.RemoveListener(OnTextCommit);
            keyboard.BackspaceEvent.RemoveListener(OnBackspace);
            keyboard.KeyboardHiddenEvent.RemoveListener(OnKeyboardHidden);
        }

        if (inputField != null)
            inputField.onSelect.RemoveListener(OnInputSelected);
    }
}