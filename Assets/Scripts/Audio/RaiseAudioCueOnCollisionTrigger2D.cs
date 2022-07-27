using UnityEngine;

public class RaiseAudioCueOnCollider2D : MonoBehaviour
{
    [Header("Collision configuration")]
    [SerializeField] private GameObject OnCollideWith = default;

    [Header("Sound definition")]
    [SerializeField] private AudioCueSO _audioCue = default;

    [Header("Configuration")]
    [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with trigger event");

        if (collision.gameObject == OnCollideWith)
        {
            Debug.Log($"Playing collider audio cue due to collision with {OnCollideWith.gameObject.name}...");
            _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration);
        }
    }
}
