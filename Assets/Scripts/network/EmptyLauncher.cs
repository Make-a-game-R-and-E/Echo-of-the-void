using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class demonstrates the most basic procedure for launching Fusion NetworkRunner.
/// INetworkRunnerCallbacks is an interface that contains all "On*" methods relevant to connecting to Fusion Network Runner.
/// </summary>
public class EmptyLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    // UI References
    [Header("UI References")]
    [SerializeField] private TMP_InputField _roomNameInput = null;
    [SerializeField] private Canvas _mainMenuCanvas = null; // Reference to the main menu Canvas
    [SerializeField] private TMP_Text _errorMessageText = null; // Reference to the error message Text
    [SerializeField] private GameObject[] _uiElementsToDisable = null; // Array to reference multiple UI elements

    protected NetworkRunner _runner;

    // Called by the "Start" button in this menu
    public void StartShared()
    {
        // Clear any existing error messages
        ClearErrorMessage();

        // If the user didn’t type anything in the InputField, pick a default name
        string roomName = _roomNameInput.text;
        if (string.IsNullOrWhiteSpace(roomName))
            roomName = "DefaultRoom";

        // Launch in Shared mode
        StartGame(GameMode.Shared, roomName);
    }

    protected async void StartGame(GameMode mode, string sessionName)
    {
        Debug.Log($"Starting game in mode {mode}, session {sessionName}");

        // Disable UI elements to prevent further input
        DisableUIElements();

        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Register callbacks
        _runner.AddCallbacks(this);

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });
    }

    /// <summary>
    /// Disables specified UI elements when the game starts.
    /// </summary>
    private void DisableUIElements()
    {
        if (_mainMenuCanvas != null)
        {
            _mainMenuCanvas.enabled = false; // Hides the entire Canvas
        }

        // Alternatively, disable individual UI elements
        if (_uiElementsToDisable != null)
        {
            foreach (var uiElement in _uiElementsToDisable)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(false); // Hides each specified UI element
                }
            }
        }
    }

    /// <summary>
    /// Re-enables the UI elements and shows an error message.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    private void ShowErrorMessage(string message)
    {
        if (_errorMessageText != null)
        {
            _errorMessageText.text = message;
            _errorMessageText.gameObject.SetActive(true);
        }

        // Re-enable UI elements so the player can try again
        if (_mainMenuCanvas != null)
        {
            _mainMenuCanvas.enabled = true; // Show the main menu Canvas
        }

        if (_uiElementsToDisable != null)
        {
            foreach (var uiElement in _uiElementsToDisable)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(true); // Show each specified UI element
                }
            }
        }
    }

    /// <summary>
    /// Clears any existing error messages.
    /// </summary>
    private void ClearErrorMessage()
    {
        if (_errorMessageText != null)
        {
            _errorMessageText.text = string.Empty;
            _errorMessageText.gameObject.SetActive(false);
        }
    }

    #region INetworkRunnerCallbacks Implementation

    public virtual void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerJoined {player}");
    }

    public virtual void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerLeft {player}");
    }

    public virtual void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log($"OnInput {input}");
    }

    public virtual void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log($"OnInputMissing {player} {input}");
    }

    public virtual void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"OnShutdown reason={shutdownReason}");
    }

    public virtual void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"OnConnectedToServer");
    }

    public virtual void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log($"OnDisconnectedFromServer {reason}");
        ShowErrorMessage("Disconnected from server. Please try again.");
    }

    public virtual void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log($"OnConnectRequest {request} {token}");
    }

    public virtual void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"OnConnectFailed {remoteAddress} {reason}");

        if (reason == NetConnectFailedReason.ServerFull)
        {
            ShowErrorMessage("Room is full. Please try another room.");
        }
        else
        {
            ShowErrorMessage($"Failed to connect: {reason}");
        }
    }

    public virtual void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log($"OnUserSimulationMessage {message}");
    }

    public virtual void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log($"OnSessionListUpdated {sessionList}");
    }

    public virtual void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log($"OnCustomAuthenticationResponse {data}");
    }

    public virtual void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log($"OnHostMigration {hostMigrationToken}");
    }

    public virtual void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log($"OnSceneLoadDone");
    }

    public virtual void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log($"OnSceneLoadStart");
    }

    public virtual void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log($"OnObjectExitAOI {obj} {player}");
    }

    public virtual void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log($"OnObjectEnterAOI {obj} {player}");
    }

    public virtual void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log($"OnReliableDataReceived {player} {key} {data}");
    }

    public virtual void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log($"OnReliableDataProgress {player} {key} {progress}");
    }

    #endregion
}
