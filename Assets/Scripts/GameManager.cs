using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [Header("Listening on channels")]
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to kill the player")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to update the high score")]
    [SerializeField] private FloatEventChannelSO _newScoreChannel = default;

    [Header("Broadcasting on channels")]
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to pause the application (if unpaused)")]
    [SerializeField] private VoidEventChannelSO _pauseApplicationChannel = default;
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to resume the application (if paused)")]
    [SerializeField] private VoidEventChannelSO _resumeApplicationChannel = default;
    [Tooltip("This event is fired when the high score has been updated.")]
    [SerializeField] private FloatEventChannelSO _highScoreUpdatedChannel = default;
    
    private float _score = 0;

    private void Awake()
    {
        _score = PlayerPrefs.GetFloat("HighScore");
    }

    private void OnEnable()
    {
        _gameOverChannel.OnEventRaised += GameOver;
        _newScoreChannel.OnEventRaised += UpdateScore;

    }

    private void UpdateScore(float score)
    {
        if (!(score > _score))
        {
            return;
        }

        PlayerPrefs.SetFloat("HighScore", score);
        _highScoreUpdatedChannel.RaiseEvent(score);
    }

    private void OnDestroy()
    {
        _gameOverChannel.OnEventRaised -= GameOver;
    }

    private void ResumeGame()
    {
        _resumeApplicationChannel.RaiseEvent();
    }

    private void GameOver()
    {
        _pauseApplicationChannel.RaiseEvent();
    }
}