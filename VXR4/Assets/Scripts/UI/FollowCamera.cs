using UnityEngine;

/*
This script does not work, either fix it or delete it.
The idea is to force text to follow the camera
This is used so we can have subtitles in VR that are always in front of the user.
*/


public class FollowCamera : MonoBehaviour {
    private OVRCameraRig ovrCamera;
    public float distance = 0.5f;
    
    void Start() {
        ovrCamera = FindObjectOfType<OVRCameraRig>();
    }
    
    void Update() {
        Transform centerEye = ovrCamera.centerEyeAnchor;
        transform.position = centerEye.position + centerEye.forward * distance;
        transform.rotation = centerEye.rotation;
    }
}