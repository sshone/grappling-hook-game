using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                StartCoroutine(UnloadYourAsyncScene(scene.name));
            }
        }

        StartCoroutine(LoadYourAsyncScene(sceneToLoad));

        _onSceneReady.RaiseEvent();
    }

    IEnumerator UnloadYourAsyncScene(string sceneName)
    {
        var asyncLoad = SceneManager.UnloadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
