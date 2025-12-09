using UnityEngine;

public class CanvasInteractionManager : MonoBehaviour
{
    public Canvas[] canvases; // Assign canvases in inspector
    private int currentIndex = 0;

    void Start()
    {
        ShowCanvas(currentIndex);
    }

    public void ShowCanvas(int index)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].gameObject.SetActive(i == index);
        }
        currentIndex = index;
        OnCanvasActivated(index);
    }

    public void NextCanvas()
    {
        int next = (currentIndex + 1) % canvases.Length;
        ShowCanvas(next);
    }

    public void PreviousCanvas()
    {
        int prev = (currentIndex - 1 + canvases.Length) % canvases.Length;
        ShowCanvas(prev);
    }

    protected virtual void OnCanvasActivated(int index)
    {
        // Override or subscribe for custom logic when a canvas is shown
        Debug.Log("Canvas " + index + " activated.");
    }
}