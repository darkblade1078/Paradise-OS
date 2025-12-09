using UnityEngine;

public class SpinBall : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}