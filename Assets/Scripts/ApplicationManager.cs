using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    [Header("Listening on channels")]
    [Tooltip("The ApplicationManager listens to this event, fired by objects in any scene, to exit the application")]
    [SerializeField] private VoidEventChannelSO _exitApplicationChannel = default;

    private void OnEnable()
    {
        _exitApplicationChannel.OnEventRaised += ExitApplication;

    }

    private void OnDestroy()
    {
        _exitApplicationChannel.OnEventRaised -= ExitApplication;
    }

    private void ExitApplication()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}
