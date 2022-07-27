using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowsCamera : MonoBehaviour
{
    public Camera _cameraToTrack;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(_cameraToTrack.transform.position.x, transform.position.y);
    }
}
