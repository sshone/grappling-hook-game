using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Behavioural
{
    public class EventsToInvokeOnDestroyBehaviour : MonoBehaviour
    {
        public UnityEvent DeathEffects;

        public void DoDeathEffects()
        {
            DeathEffects.Invoke();
        }
    }
}