using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button_RaiseAudioCueEventOnClick : MonoBehaviour
{
    private Button _button;

    [Header("Sound definition")]
    [SerializeField] private AudioCueSO _audioCue = default;

    [Header("Configuration")]
    [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;

    void Start()
    {
        
        if (!TryGetComponent(out _button))
        {
            Debug.LogError("No button component found");
            return;
        }

        _button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration);
    }
}
