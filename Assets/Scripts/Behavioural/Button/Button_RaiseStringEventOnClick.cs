using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button_RaiseStringEventOnClick : MonoBehaviour
{
    private Button _button;

    [Header("Configuration")]
    [SerializeField] private string _sceneName;
    [SerializeField] private StringEventChannelSO _sceneEventChannel = default;

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
        _sceneEventChannel.RaiseEvent(_sceneName);
    }
}
