using UnityEngine;

public class PlayerMovementTracker : MonoBehaviour
{
    [Header("Hand Tracking")]
    public OVRHand leftHand;
    public OVRHand rightHand;

    [Header("Head Tracking")]
    public Transform headTransform; // Assign CenterEyeAnchor here

    [Header("Movement Settings")]
    public float movementThreshold = 0.01f; // Minimum distance to consider as movement

    public bool IsLeftHandMoving { get; private set; }
    public bool IsRightHandMoving { get; private set; }
    public bool IsBodyMoving { get; private set; }

    private Vector3 prevLeftHandPos;
    private Vector3 prevRightHandPos;
    private Vector3 prevHeadPos;

    void Start()
    {
        if (leftHand != null) prevLeftHandPos = leftHand.transform.position;
        if (rightHand != null) prevRightHandPos = rightHand.transform.position;
        if (headTransform != null) prevHeadPos = headTransform.position;
    }

    void Update()
    {
        if (leftHand != null)
        {
            float leftHandDist = Vector3.Distance(leftHand.transform.position, prevLeftHandPos);
            IsLeftHandMoving = leftHandDist > movementThreshold;
            prevLeftHandPos = leftHand.transform.position;
        }

        if (rightHand != null)
        {
            float rightHandDist = Vector3.Distance(rightHand.transform.position, prevRightHandPos);
            IsRightHandMoving = rightHandDist > movementThreshold;
            prevRightHandPos = rightHand.transform.position;
        }

        if (headTransform != null)
        {
            float headDist = Vector3.Distance(headTransform.position, prevHeadPos);
            IsBodyMoving = headDist > movementThreshold;
            prevHeadPos = headTransform.position;
        }
    }
}
