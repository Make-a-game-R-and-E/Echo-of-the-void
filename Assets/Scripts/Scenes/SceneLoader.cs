using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    /// <summary>
    /// Loads the scene by the given name if it is not empty.
    /// </summary>
    public static void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty. Please specify a scene name.");
        }
    }

    /// <summary>
    /// Reloads the currently active scene.
    /// </summary>
    public static void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
