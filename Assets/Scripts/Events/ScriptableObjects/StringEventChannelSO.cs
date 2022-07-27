using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one String argument.
/// </summary>
[CreateAssetMenu(menuName = "Events/String Event Channel")]
public class StringEventChannelSO : DescriptionBaseSO
{
    public UnityAction<string> OnEventRaised;

    public void RaiseEvent(string value)
    {
        OnEventRaised?.Invoke(value);
    }
}