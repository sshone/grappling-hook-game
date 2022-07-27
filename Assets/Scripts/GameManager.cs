using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Listening on channels")]
    [Tooltip("The GameManager listens to this event, fired by objects in any scene, to kill the player")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;

    [Header("Broadcasting on channels")]
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to pause the application (if unpaused)")]
    [SerializeField] private VoidEventChannelSO _pauseApplicationChannel = default;
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to resume the application (if paused)")]
    [SerializeField] private VoidEventChannelSO _resumeApplicationChannel = default;

    private void OnEnable()
    {
        _gameOverChannel.OnEventRaised += GameOver;

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