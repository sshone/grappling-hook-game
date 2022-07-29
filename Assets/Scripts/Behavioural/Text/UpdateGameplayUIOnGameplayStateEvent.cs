using Assets.Scripts.Common;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UpdateGameplayUIOnGameplayStateEvent : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText = null;
    [SerializeField]
    private TMP_Text _highScoreText = null;
    [SerializeField]
    private TMP_Text _gameOverScoreText = null;

    [Header("Listening on channels")]
    [Tooltip("Listens for an event on this channel to set GameObject to active")]
    [SerializeField] private GameplayStateEventChannelSO _gameplayStateEventChannel = default;

    private void OnEnable()
    {
        _gameplayStateEventChannel.OnEventRaised += SetTextToFloat;

    }

    private void OnDestroy()
    {
        _gameplayStateEventChannel.OnEventRaised -= SetTextToFloat;
    }

    private void SetTextToFloat(GameplayStateArgs value)
    {
        Debug.Log("Updating gameplay ui");

        _scoreText.text = value.Score.ToString(CultureInfo.InvariantCulture);
        _gameOverScoreText.text = value.Score.ToString(CultureInfo.InvariantCulture);
        _highScoreText.text = value.HighScore.ToString(CultureInfo.InvariantCulture);
    }
}
