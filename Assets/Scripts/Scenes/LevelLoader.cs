using UnityEngine;
using Fusion; // Photon Fusion namespace
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Scene To Load")]
    [Tooltip("The name of the scene you want to load")]
    [SerializeField] string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Attempt to get the NetworkObject component from the collided player
            NetworkObject networkObject = collision.gameObject.GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                // Check if this NetworkObject has input authority on this client
                if (networkObject.HasInputAuthority)
                {
                    if (!string.IsNullOrEmpty(levelName))
                    {
                        // Initiate scene loading for this client only
                        LoadSceneForLocalPlayer(levelName);
                    }
                    else
                    {
                        Debug.LogWarning("Level name is empty. Please set the level name in the inspector.");
                    }
                }
                else
                {
                    Debug.Log("Collided player does not have input authority on this client. No action taken.");
                }
            }
            else
            {
                Debug.LogWarning("Collided object does not have a NetworkObject component.");
            }
        }
    }

    /// Loads the specified scene for the local player only.
    /// <param name="sceneName">Name of the scene to load.</param>
    private void LoadSceneForLocalPlayer(string sceneName)
    {
        // Using Unity's SceneManager for client-side scene loading
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        Debug.Log($"Local player initiating scene load: {sceneName}");
    }
}
