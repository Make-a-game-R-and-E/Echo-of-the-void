using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Fusion;
using System.Collections.Generic;

/// <summary>
/// This script should be on your "Start Menu" scene. 
/// It creates (or finds) a NetworkRunner and starts the session in Shared mode,
/// telling Fusion to load your Game scene when ready.
/// </summary>
public class StartMenu : MonoBehaviour
{
    [Header("Network Runner Prefab (optional)")]
    // If you have a prefab with a NetworkRunner on it, assign it here;
    // otherwise, we can create one at runtime.
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;

    [Header("UI References")]
    [SerializeField] TMP_InputField _roomNameInput = null;

    [Header("Scene Settings")]
    // The Build Index of the actual Game scene you want to load
    [SerializeField] private int _gameSceneIndex = 2;

    // Keep a reference to our instantiated runner (so we don't create duplicates)
    private NetworkRunner _runnerInstance = null;

    // Called by the "Start" button in this menu
    public void StartShared()
    {
        // If the user didn’t type anything in the InputField, pick a default name
        string roomName = _roomNameInput.text;
        if (string.IsNullOrWhiteSpace(roomName))
            roomName = "DefaultRoom";

        // Launch in Shared mode
        StartGame(GameMode.Shared, roomName);
    }

    private async void StartGame(GameMode mode, string roomName)
    {
        // Look for an existing runner in case we are coming back or something
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
        {
            // If we have a prefab, use it; else, create a brand new GO
            if (_networkRunnerPrefab != null)
            {
                _runnerInstance = Instantiate(_networkRunnerPrefab);
            }
            else
            {
                GameObject runnerGO = new GameObject("NetworkRunner");
                _runnerInstance = runnerGO.AddComponent<NetworkRunner>();
            }
        }

        // Make sure the runner isn't destroyed when loading the new scene
        DontDestroyOnLoad(_runnerInstance.gameObject);

        // We want to send local input from this client
        _runnerInstance.ProvideInput = true;

        // If there's no default Scene Manager attached, add it
        // This helps Fusion handle scene loading/unloading automatically
        if (_runnerInstance.GetComponent<NetworkSceneManagerDefault>() == null)
        {
            _runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        // Prepare the StartGameArgs
        var startArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            // This tells Fusion to automatically load the Game scene (index = _gameSceneIndex)
            Scene = SceneRef.FromIndex(_gameSceneIndex),
            SceneManager = _runnerInstance.GetComponent<NetworkSceneManagerDefault>(),

            // Example: limit session to 2 players
            SessionProperties = new Dictionary<string, SessionProperty>()
            {
                { "MaxPlayers", 2 }
            }
        };

        // Kick off the session
        await _runnerInstance.StartGame(startArgs);

        // IMPORTANT:
        // Do NOT call _runnerInstance.LoadScene(...) again if you already specified
        // the scene in StartGameArgs above. That can cause double-loading or conflicts.
        //
        // If for some reason you must manually load, it would look like this:
        // if (_runnerInstance.IsServer)
        //     _runnerInstance.SetActiveScene("NameOfYourGameScene");
    }
}
