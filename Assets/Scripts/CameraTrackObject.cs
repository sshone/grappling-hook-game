using UnityEngine;

public class CameraTrackObject : MonoBehaviour
{
    public Transform _trackingObject;

    // Update is called once per frame
    void Update()
    {
        transform.position = _trackingObject.position;
    }
}
