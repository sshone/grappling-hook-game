using UnityEngine;

public class RaiseAudioCueOnCollisionTrigger2D : MonoBehaviour
{
    [Header("Collision configuration")]
    [SerializeField] private Collider2D OnCollideWith = default;

    [Header("Sound definition")]
    [SerializeField] private AudioCueSO _audioCue = default;

    [Header("Configuration")]
    [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with trigger event");

        if (collision == OnCollideWith)
        {
            Debug.Log($"Playing collider audio cue due to collision with {OnCollideWith.name}...");
            _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration);
        }
    }
}
