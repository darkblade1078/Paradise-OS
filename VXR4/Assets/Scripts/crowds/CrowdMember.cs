using UnityEngine;

public class CrowdMember : MonoBehaviour
{
    private float baseY;
    public float bounceSpeed = 1.5f;
    private float bobHeight;
    private float bobOffset;

    void Start()
    {
        baseY = transform.position.y;
        bobHeight = Random.Range(0.1f, 0.4f);
        bobOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float y = baseY + Mathf.Sin(Time.time * bounceSpeed + bobOffset) * bobHeight;
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
}
