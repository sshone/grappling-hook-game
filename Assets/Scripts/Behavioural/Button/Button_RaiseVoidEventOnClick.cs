using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button_RaiseVoidEventOnClick : MonoBehaviour
{
    private Button _button;

    [Header("Configuration")]
    [SerializeField] private VoidEventChannelSO _voidEventChannel = default;

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
        _voidEventChannel.RaiseEvent();
    }
}
