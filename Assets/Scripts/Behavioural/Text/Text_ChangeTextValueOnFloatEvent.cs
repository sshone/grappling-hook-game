using System.Globalization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Text_ChangeTextValueOnGameplayStateEvent : MonoBehaviour
{
    private TMP_Text _text;

    [Header("Listening on channels")]
    [Tooltip("Listens for an event on this channel to set GameObject to active")]
    [SerializeField] private FloatEventChannelSO _floatEventChannel = default;

    void Start()
    {

        if (!TryGetComponent(out _text))
        {
            Debug.LogError("No text component found");
        }
    }

    private void OnEnable()
    {
        _floatEventChannel.OnEventRaised += SetTextToFloat;

    }

    private void OnDestroy()
    {
        _floatEventChannel.OnEventRaised -= SetTextToFloat;
    }

    private void SetTextToFloat(float value)
    {
        _text.text = value.ToString(CultureInfo.InvariantCulture);
    }
}
