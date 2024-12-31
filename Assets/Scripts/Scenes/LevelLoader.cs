using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Scene To Load")]
    [Tooltip("The name of the scene you want to load")]
    [SerializeField] string levelName;

    [Tooltip("The build index of the scene you want to load")]
    [SerializeField] int levelIndex;

    /// Loads a scene by the specified name.
    /// Ensure the scene is added in the Build Settings.
    public void LoadLevelByName()
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogWarning("Level name is empty. Please set the level name in the inspector.");
        }
    }

    /// Loads a scene by the specified build index.
    /// Ensure the scene is added in the Build Settings.
    public void LoadLevelByIndex()
    {
        // Make sure the index is valid
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogWarning("Invalid level index. Make sure the scene is added to Build Settings and index is correct.");
        }
    }
}
