using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button_RaiseFloatEventOnClick : MonoBehaviour
{
    private Button _button;

    [Header("Variable definition")]
    [SerializeField] private float _floatValue = default;

    [Header("Configuration")]
    [SerializeField] private FloatEventChannelSO _floatEventChannel = default;

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
        _floatEventChannel.RaiseEvent(_floatValue);
    }
}
