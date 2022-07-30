using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowsCamera : MonoBehaviour
{
    public bool MaintainXOffset = false;
    public bool MaintainYOffset = false;

    private float xOffset = 0f;
    private float yOffset = 0f;

    public Camera _cameraToTrack;

    void Start()
    {
        if (MaintainXOffset)
        {
            xOffset = transform.position.x - _cameraToTrack.transform.position.x;
        }

        if (MaintainYOffset)
        {
            yOffset = transform.position.y - _cameraToTrack.transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
            

            if (MaintainYOffset)
            {
                transform.position = new Vector2(_cameraToTrack.transform.position.x + xOffset,
                                                 _cameraToTrack.transform.position.y + yOffset);
            }
            else
            {
                transform.position = new Vector2(_cameraToTrack.transform.position.x + xOffset,
                                                 transform.position.y);
        }
    }
}
