using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Behavioural
{
    public class EventsToInvokeOnVoidChannelEventBehaviour : MonoBehaviour
    {
        [Header("Listening on channels")]
        [Tooltip("Listens for an event on this channel to fire off configured events")]
        [SerializeField] private VoidEventChannelSO _voidEventChannel = default;

        
        public UnityEvent VoidChannelEvents;

        private void OnEnable()
        {
            _voidEventChannel.OnEventRaised += DoEffects;

        }

        private void OnDestroy()
        {
            _voidEventChannel.OnEventRaised -= DoEffects;
        }

        private void DoEffects()
        {
            VoidChannelEvents.Invoke();
        }
    }
}