using System;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private LayerMask grappleableLayer;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Sound definition")]
    [SerializeField] private AudioCueSO _audioCue = default;

    [Header("Configuration")]
    [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [HideInInspector] public GameObject grappledObject;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        if (grappledObject == null)
        {
            ReleaseGrapple();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            UpdateGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ReleaseGrapple();
        }
        else
        {
            RotateGunToMouse();
        }
    }

    private void ReleaseGrapple()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        m_rigidbody.gravityScale = 1;
    }

    private void UpdateGrapple()
    {
        if (grappleRope.enabled)
        {
            RotateGun(grapplePoint, false);
        }
        else
        {
            RotateGunToMouse();
        }

        if (!launchToPoint || !grappleRope.isGrappling)
        {
            return;
        }

        if (launchType == LaunchType.Transform)
        {
            Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
            var targetPos = grapplePoint - firePointDistance;
            gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
        }
    }

    private void RotateGunToMouse()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        RotateGun(mousePos, true);
    }

    private void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        var distanceVector = lookPoint - gunPivot.position;

        var angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward),
                                                Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        var hit = Physics2D.Raycast(firePoint.position, distanceVector, 50, grappleableLayer);

        if (!hit)
        {
            return;
        }

        if (!(Vector2.Distance(hit.point, firePoint.position) <= maxDistance) && hasMaxDistance)
        {
            return;
        }

        PlayGrappleSound();

        grapplePoint = hit.point;
        grappleDistanceVector = grapplePoint - (Vector2) gunPivot.position;
        grappledObject = hit.transform.gameObject;
        grappleRope.enabled = true;
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }

        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void PlayGrappleSound()
    {
        _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration);
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null || !hasMaxDistance)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(firePoint.position, maxDistance);
    }

    private enum LaunchType
    {
        Transform,
        Physics
    }
}