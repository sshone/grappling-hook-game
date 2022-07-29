using UnityEngine;

namespace Assets.Scripts
{
    internal class OnAwakeRaiseVoidEvent : MonoBehaviour
    {
        [Header("Broadcasting on channels")]
        [Tooltip("Raises event on this channel when the object awakens")]
        [SerializeField] private VoidEventChannelSO _voidEventChannel = default;

        private void Awake ()
        {
            _voidEventChannel.RaiseEvent();
        }
    }
}
