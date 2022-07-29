using UnityEngine;

namespace Assets.Scripts.Behavioural
{
    public class IncreaseScoreBehaviour : MonoBehaviour
    {
        [Header("Configuration")] [Tooltip("The amount to increase the score by")] [SerializeField]
        private float _scoreValue;

        [Header("Broadcasting on")] [Tooltip("Fires an event on this channel to increase score")] [SerializeField]
        private FloatEventChannelSO _scoreIncreaseChannel = default;

        public void IncreaseScore() => _scoreIncreaseChannel.RaiseEvent(_scoreValue);
    }
}