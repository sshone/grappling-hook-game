using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Behavioural
{
    public class EventsToInvokeOnGameplayStateChangeChannelEventBehaviour : MonoBehaviour
    {
        [Header("Listening on channels")]
        [Tooltip("Listens for an event on this channel to fire off configured events")]
        [SerializeField] private GameplayStateEventChannelSO _eventChannel = default;

        public UnityEvent VoidChannelEvents;

        private void OnEnable()
        {
            _eventChannel.OnEventRaised += DoEffects;

        }

        private void OnDestroy()
        {
            _eventChannel.OnEventRaised -= DoEffects;
        }

        private void DoEffects(GameplayStateArgs value)
        {
            VoidChannelEvents.Invoke();
        }
    }
}