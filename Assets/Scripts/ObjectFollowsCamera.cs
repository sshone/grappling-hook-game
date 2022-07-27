using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowsCamera : MonoBehaviour
{
    public bool MaintainXOffset = false;

    private float xOffset = 0f;

    public Camera _cameraToTrack;

    void Start()
    {
        xOffset = transform.position.x - _cameraToTrack.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = MaintainXOffset
                                 ? new Vector2(_cameraToTrack.transform.position.x + xOffset,
                                               transform.position.y)
                                 : new Vector2(_cameraToTrack.transform.position.x,
                                               transform.position.y);
    }
}
