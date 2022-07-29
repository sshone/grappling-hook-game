using UnityEngine;

namespace Assets.Scripts.Behavioural
{
    public class EmitAudioCueBehaviour : MonoBehaviour
    {
        [Header("Sound definition")]
        [SerializeField] private AudioCueSO _audioCue = default;

        [Header("Configuration")]
        [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
        [SerializeField] private AudioConfigurationSO _audioConfiguration = default;

        public void PlayAudioClip()
        {
            _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration);
        }
    }
}