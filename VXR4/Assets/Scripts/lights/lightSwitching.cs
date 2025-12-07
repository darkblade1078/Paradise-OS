using UnityEngine;

public class LightSwitching : MonoBehaviour
{
    public Light[] lights; // Assign 4 lights in inspector
    public float switchInterval = 1f; // Seconds between color changes
    private float timer;

    void Start()
    {
        if (lights == null || lights.Length != 4)
        {
            Debug.LogWarning("Please assign exactly 4 lights to the LightSwitching script.");
        }
        timer = switchInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChangeLightsColor();
            timer = switchInterval;
        }
    }

    void ChangeLightsColor()
    {
        for (int i = 0; i < lights.Length && i < 4; i++)
        {
            // Avoid white (V=1, S=0) and black (V=0)
            lights[i].color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f); // S and V never 0
        }
    }
}
