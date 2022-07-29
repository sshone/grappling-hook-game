using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
	[Header("Listening to")]
	[SerializeField] private StringEventChannelSO _sceneLoader = default;

	[Header("Broadcasting on")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

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
                StartCoroutine(UnloadYourAsyncScene(scene.name));
            }
        }

        StartCoroutine(LoadYourAsyncScene(sceneToLoad));

        _onSceneReady.RaiseEvent();
    }

    IEnumerator UnloadYourAsyncScene(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
