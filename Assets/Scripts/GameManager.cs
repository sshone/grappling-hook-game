using Assets.Scripts.Common;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Listening on channels")]
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to kill the player")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to increase the current scoreIncreaseValue")]
    [SerializeField] private FloatEventChannelSO _scoreIncreasedChannel = default;
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to send a UI Update event")]
    [SerializeField] private VoidEventChannelSO _gameStateRequestedChannel = default;

    [Header("Broadcasting on channels")]
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to pause the application (if unpaused)")]
    [SerializeField] private VoidEventChannelSO _pauseApplicationChannel = default;
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to resume the application (if paused)")]
    [SerializeField] private VoidEventChannelSO _resumeApplicationChannel = default;
    [Tooltip("This event is fired when the Gameplay State has been updated.")]
    [SerializeField] private GameplayStateEventChannelSO _gameplayStateChangedChannel = default;
    
    private float _currentScore = 0;
    private float _highScore;

    private void Awake()
    {
        _currentScore = 0;
        _highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    private void OnEnable()
    {
        _gameOverChannel.OnEventRaised += GameOver;
        _scoreIncreasedChannel.OnEventRaised += UpdateScore;
        _gameStateRequestedChannel.OnEventRaised += RaiseGameplayStateUpdateEvent;

    }

    private void UpdateScore(float scoreIncreaseValue)
    {
        Debug.Log("updating score");

        _currentScore += scoreIncreaseValue;

        if (_currentScore > _highScore)
        {
            PlayerPrefs.SetFloat("HighScore", scoreIncreaseValue);
            _highScore = _currentScore;
        }

        RaiseGameplayStateUpdateEvent();
    }

    private void RaiseGameplayStateUpdateEvent()
    {
        Debug.Log($"Raising gameplay update event {_currentScore} and {_highScore}");
        _gameplayStateChangedChannel.RaiseEvent(new GameplayStateArgs(_currentScore, _highScore));
    }

    private void OnDestroy()
    {
        _gameOverChannel.OnEventRaised -= GameOver;
        _scoreIncreasedChannel.OnEventRaised -= UpdateScore;
        _gameStateRequestedChannel.OnEventRaised -= RaiseGameplayStateUpdateEvent;
    }

    private void ResumeGame()
    {
        _resumeApplicationChannel.RaiseEvent();
    }

    private void GameOver()
    {
        _pauseApplicationChannel.RaiseEvent();
        _currentScore = 0;
    }
}