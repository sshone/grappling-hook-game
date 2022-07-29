using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one GameplayStateArgs argument.
/// </summary>
[CreateAssetMenu(menuName = "Events/GameplayState Event Channel")]
public class GameplayStateEventChannelSO : DescriptionBaseSO
{
    public UnityAction<GameplayStateArgs> OnEventRaised;

    public void RaiseEvent(GameplayStateArgs value)
    {
        OnEventRaised?.Invoke(value);
    }
}