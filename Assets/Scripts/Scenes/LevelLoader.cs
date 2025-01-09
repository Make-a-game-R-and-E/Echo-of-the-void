using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Scene To Load")]
    [Tooltip("The name of the scene you want to load")]
    [SerializeField] string levelName;


    /// Loads a scene by the specified name.
    /// Ensure the scene is added in the Build Settings.
    public void LoadLevelByName()
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            SceneLoader.LoadSceneByName(levelName);
        }
        else
        {
            Debug.LogWarning("Level name is empty. Please set the level name in the inspector.");
        }
    }
}
