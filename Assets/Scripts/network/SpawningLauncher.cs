using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

/// <summary>
/// This script should be placed in your "Game" scene (the 3rd scene).
/// Once that scene is loaded, it will handle actual Player spawning.
/// </summary>
public class SpawningLauncher : EmptyLauncher
{
    [Header("Player Prefab & Spawn Points")]
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    // Keep track of spawned player objects
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    [Header("Player Movement Input")]
    [SerializeField] private InputAction moveAction = new InputAction(type: InputActionType.Value);

    private NetworkInputData _inputData;

    private void OnEnable()
    {
        // Enable the new Input System Action so we can read movement input
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    // Called every time a new player joins (in Shared or Host mode)
    public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player} joined.");

        // In Shared mode: each local client spawns its own player
        // In Host/Server mode: only the server spawns for all
        bool isAllowedToSpawn = (runner.GameMode == GameMode.Shared)
            ? (player == runner.LocalPlayer)
            : runner.IsServer;

        if (isAllowedToSpawn)
        {
            // pick a spawn point (mod in case more players than points)
            Transform chosenSpawn = spawnPoints[player.PlayerId % spawnPoints.Length];
            Vector3 spawnPos = chosenSpawn.position;
            Quaternion spawnRot = chosenSpawn.rotation;

            // spawn the player prefab, giving the new player input authority
            NetworkObject networkPlayerObject = runner.Spawn(
                _playerPrefab,
                spawnPos,
                spawnRot,
                player
            );

            // Keep track so we can despawn if they leave
            _spawnedCharacters[player] = networkPlayerObject;
        }
    }

    // Called if a player leaves/disconnects
    public override void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player} left.");
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    // Called every network tick on the local client to gather input
    public override void OnInput(NetworkRunner runner, NetworkInput input)
    {
        _inputData.moveActionValue = moveAction.ReadValue<Vector2>();
        input.Set(_inputData);
    }
}
