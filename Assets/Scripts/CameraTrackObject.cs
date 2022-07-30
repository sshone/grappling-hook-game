using UnityEngine;

public class CameraTrackObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Target;
    [SerializeField]
    private Camera Camera;

    [Header("Smooth Follow Configuration")]
    [SerializeField]
    private float DampTime = 0.15f;

    //public float targetZoom, zoomFactor, zoomLerpSpeed;
    [Header("Velocity Zoom Configuration")]
    [SerializeField]
    [Range(5, 20)]
    private float minZoom = 5f;
    [SerializeField] 
    [Range(5, 20)] 
    private float standardZoom = 10f;
    [SerializeField]
    [Range(5, 20)]
    private float maxZoom = 15f;
    [SerializeField]
    [Range(0, 1)]
    private float smoothTime = 1f;

    [SerializeField]
    private float velocityToStartZoomOut = 20f;
    [SerializeField]
    private float velocityToStartZoomIn = 10f;
    
    private Vector3 _velocity = Vector3.zero;
    private float _velocity2;
    private float velocityTest;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Target)
        {
            return;
        }

        SmoothFollow();
        SmoothZoom();
    }

    public void SmoothFollow()
    {
        var point = Camera.WorldToViewportPoint(Target.position);
        var delta = (Vector3) Target.position - Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        var destination = transform.position + delta;

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, DampTime);
    }

    public void SmoothZoom()
    {
        var velocity = Target.velocity.sqrMagnitude;

        if (velocity > velocityToStartZoomOut)
        {
            SmoothZoomOut();
        } else if (velocity < velocityToStartZoomIn)
        {
            SmoothZoomIn();
        }
        else
        {
            SmoothToStandard();
        }

        velocityTest = velocity;
    }

    public void SmoothZoomOut()
    {
        Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, maxZoom, ref _velocity2, smoothTime);
    }

    public void SmoothZoomIn()
    {
        Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, minZoom, ref _velocity2, smoothTime);
    }

    public void SmoothToStandard()
    {
        Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, standardZoom, ref _velocity2, smoothTime);
    }
}
