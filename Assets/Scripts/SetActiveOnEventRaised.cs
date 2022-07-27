using UnityEngine;

public class SetActiveOnEventRaised : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The GameObject to set to active")]
    [SerializeField]
    private GameObject _objectToActivate;

    [Header("Listening on channels")]
    [Tooltip("Listens for an event on this channel to set GameObject to active")]
    [SerializeField] private VoidEventChannelSO _voidEventChannel = default;

    private void OnEnable()
    {
        _voidEventChannel.OnEventRaised += SetGameObjectActive;

    }

    private void OnDestroy()
    {
        _voidEventChannel.OnEventRaised -= SetGameObjectActive;
    }

    private void SetGameObjectActive()
    {
        _objectToActivate.SetActive(true);
    }
}
