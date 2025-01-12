using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class AllPlayersLevelLoader : NetworkBehaviour
{
    [Header("Scene To Load")]
    [Tooltip("The name of the scene to load when all players are present")]
    [SerializeField] private string targetSceneName;


    [SerializeField] int numberOfPlayers = 2;
    // Sum of players in the trigger zone
    private int playersInTrigger = 0;

    // Reference to the NetworkRunner
    private NetworkRunner runner;

    private void Start()
    {
        // Obtain a reference to the NetworkRunner in the scene
        runner = FindFirstObjectByType<NetworkRunner>();
        if (runner == null)
        {
            Debug.LogError("NetworkRunner not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (runner == null)
            return;

        if (collision.CompareTag("Player"))
        {
            playersInTrigger++;
            if (playersInTrigger >= numberOfPlayers)
            {
                InitiateSceneTransition();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (runner == null)
            return;

        if (collision.CompareTag("Player"))
        {
            playersInTrigger--;
        }
    }

    /// Initiates loading the next scene for all players.
    private void InitiateSceneTransition()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is not set.");
            return;
        }

        // Ensure only the server (host) initiates the scene transition
        if (runner.IsServer)
        {
            // Call the RPC to instruct all clients to load the new scene
            RPC_LoadScene(targetSceneName);
        }
    }

    /// RPC method to instruct all clients to load the specified scene.
    /// <param name="sceneName">Name of the scene to load.</param>
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_LoadScene(string sceneName)
    {
        Debug.Log($"RPC Received to Load Scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
