using UnityEngine;

public class CameraTrackObject : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Camera _camera;

    public float maxZoom = 10;
    public float minZoom = 20;
    public float sensitivity = 1;
    public float speed = 30;
    float targetZoom;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        var point = _camera.WorldToViewportPoint(target.position);
        var delta =
            target.position -
            _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        var destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        
        SmoothToValue();
    }

    public void SmoothToValue()
    {
       
    }
}
