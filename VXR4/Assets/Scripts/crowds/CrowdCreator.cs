using UnityEngine;

public class CrowdCreator : MonoBehaviour
{
    // For gizmo visualization
    private float minJumpHeight = 0.1f;
    private float maxJumpHeight = 0.4f;
    public int crowdWidth = 10;
    public int crowdDepth = 5;
    public float spacing = 0.3f; // Super close by default
    public float bounceSpeed = 1.5f; // Control bounce speed
    public GameObject crowdPrefab; // Assign a capsule/cube prefab in inspector
    [Header("Crowd Rotation")]
    public Vector3 crowdRotation = Vector3.zero;
    // Mesh combining removed; always use individual capsules

    void Start()
    {
        GenerateCrowd();
    }

    public void GenerateCrowd()
    {
        // Remove old crowd
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        Quaternion crowdRot = Quaternion.Euler(crowdRotation);
        // Create new crowd
        for (int x = 0; x < crowdWidth; x++)
        {
            for (int z = 0; z < crowdDepth; z++)
            {
                Vector3 localPos = new Vector3(x * spacing, 0, z * spacing);
                Vector3 pos = transform.position + crowdRot * localPos;
                GameObject go = Instantiate(crowdPrefab, pos, crowdRot, transform);
                // Assign random color
                Renderer rend = go.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
                }
                // Add animation and set bounce speed
                CrowdMember member = go.AddComponent<CrowdMember>();
                member.bounceSpeed = bounceSpeed;
            }
        }
    }

    [ContextMenu("Regenerate Crowd")]
    public void RegenerateCrowd() {
        GenerateCrowd();
    }
    
    // Mesh combining removed

    void OnDrawGizmos()
    {
        // Draw crowd area and jump height range with rotation
        Quaternion crowdRot = Quaternion.Euler(crowdRotation);
        Gizmos.color = Color.cyan;
        Vector3[] corners = new Vector3[4];
        corners[0] = transform.position + crowdRot * Vector3.zero;
        corners[1] = transform.position + crowdRot * new Vector3(crowdWidth * spacing, 0, 0);
        corners[2] = transform.position + crowdRot * new Vector3(crowdWidth * spacing, 0, crowdDepth * spacing);
        corners[3] = transform.position + crowdRot * new Vector3(0, 0, crowdDepth * spacing);
        // Draw area
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);

        // Draw jump height range
        Gizmos.color = Color.magenta;
        float minY = transform.position.y + minJumpHeight;
        float maxY = transform.position.y + maxJumpHeight;
        for (int i = 0; i < 4; i++)
        {
            Vector3 minCorner = new Vector3(corners[i].x, minY, corners[i].z);
            Vector3 minNext = new Vector3(corners[(i+1)%4].x, minY, corners[(i+1)%4].z);
            Gizmos.DrawLine(minCorner, minNext);
            Vector3 maxCorner = new Vector3(corners[i].x, maxY, corners[i].z);
            Vector3 maxNext = new Vector3(corners[(i+1)%4].x, maxY, corners[(i+1)%4].z);
            Gizmos.DrawLine(maxCorner, maxNext);
        }
    }
}
