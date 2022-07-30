using Assets.Scripts.Common;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay Configuration")]
    [Tooltip("The maximum grace period after acquiring a score multiplier before it resets")]
    [SerializeField]
    private float _multiplierLossGracePeriodSeconds = 1f;

    [Header("Listening on channels")]
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to kill the player")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to increase the current baseScoreValue")]
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

    private int _currentScoreMultiplier = 1;
    private float _lastScoreTime = 0f;
    
    private int _maxScoreMultiplier = 4;

    private void Awake()
    {
        _currentScore = 0;
        _highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    private void Update()
    {
        if (_currentScoreMultiplier > 1 && (Time.time - _lastScoreTime > _multiplierLossGracePeriodSeconds))
        {
            _currentScoreMultiplier = 1;
            RaiseGameplayStateUpdateEvent();
        }
    }

    private void OnEnable()
    {
        _gameOverChannel.OnEventRaised += GameOver;
        _scoreIncreasedChannel.OnEventRaised += UpdateScore;
        _gameStateRequestedChannel.OnEventRaised += RaiseGameplayStateUpdateEvent;

    }

    private void UpdateScore(float baseScoreValue)
    {
        Debug.Log("updating score");
        var scoreIncreaseValue = baseScoreValue * _currentScoreMultiplier;

        _currentScore += scoreIncreaseValue;

        if (_currentScore > _highScore)
        {
            PlayerPrefs.SetFloat("HighScore", _currentScore);
            _highScore = _currentScore;
        }

        UpdateMultiplier();
        RaiseGameplayStateUpdateEvent();
    }

    private void UpdateMultiplier()
    {
        if (_currentScoreMultiplier == 1 || ((Time.time - _lastScoreTime) < _multiplierLossGracePeriodSeconds && _currentScoreMultiplier < _maxScoreMultiplier))
        {
            _currentScoreMultiplier++;
        }
        else if (_currentScoreMultiplier != _maxScoreMultiplier && (Time.time - _lastScoreTime) > _multiplierLossGracePeriodSeconds)
        {
            _currentScoreMultiplier = 1;
        }

        _lastScoreTime = Time.time;
    }

    private void RaiseGameplayStateUpdateEvent()
    {
        Debug.Log($"Raising gameplay update event {_currentScore} and {_highScore} and {_currentScoreMultiplier}");
        _gameplayStateChangedChannel.RaiseEvent(new GameplayStateArgs(_currentScore, _highScore, _currentScoreMultiplier));
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
        _currentScoreMultiplier = 1;
    }
}