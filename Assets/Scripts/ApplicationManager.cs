using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    [Header("Listening on channels")]
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to exit the application")]
    [SerializeField] private VoidEventChannelSO _exitApplicationChannel = default;
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to pause the application (if unpaused)")]
    [SerializeField] private VoidEventChannelSO _pauseApplicationChannel = default;
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to resume the application (if paused)")]
    [SerializeField] private VoidEventChannelSO _resumeApplicationChannel = default;

    private void OnEnable()
    {
        _exitApplicationChannel.OnEventRaised += ExitApplication;
        _pauseApplicationChannel.OnEventRaised += PauseApplication;
        _resumeApplicationChannel.OnEventRaised += UnPauseApplication;

    }

    private void OnDestroy()
    {
        _exitApplicationChannel.OnEventRaised -= ExitApplication;
        _pauseApplicationChannel.OnEventRaised -= PauseApplication;
        _resumeApplicationChannel.OnEventRaised -= UnPauseApplication;
    }

    private void ExitApplication()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }

    private void PauseApplication()
    {
        Debug.Log("Pausing application...");
        Time.timeScale = 0f;
    }

    private void UnPauseApplication()
    {
        Debug.Log("Resuming application...");
        Time.timeScale = 1f;
    }
}
