using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
	[Header("Listening to")]
	[SerializeField] private StringEventChannelSO _sceneLoader = default;

	[Header("Broadcasting on")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default; //picked up by the SpawnSystem

    private void OnEnable()
    {
        _sceneLoader.OnEventRaised += LoadScene;

    }

    private void OnDestroy()
    {
        _sceneLoader.OnEventRaised -= LoadScene;
    }

    public void LoadScene(string sceneToLoad)
    {
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name != "Initialization")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        _onSceneReady.RaiseEvent();
    }
}
