using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseVoidEventOnSceneReady : MonoBehaviour
{
    [Header("Listening on channels")]
    [Tooltip("Listens for an event on this channel to fire 'OnSceneReady' event")]
    [SerializeField] private VoidEventChannelSO _voidEventChannel = default;

    [Header("Broadcasting on channels")]
    [Tooltip("Fires a void event on this channel when event received")]
    [SerializeField] private VoidEventChannelSO _broadcastEventChannel = default;

    private void OnEnable()
    {
        _voidEventChannel.OnEventRaised += Broadcast;

    }

    private void OnDestroy()
    {
        _voidEventChannel.OnEventRaised -= Broadcast;
    }

    private void Broadcast()
    {
        _broadcastEventChannel.RaiseEvent();
    }
}
