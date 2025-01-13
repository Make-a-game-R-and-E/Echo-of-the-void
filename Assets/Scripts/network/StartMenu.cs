using Fusion;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

// A utility class which defines the behaviour of the various buttons and input fields found in the Menu scene
public class StartMenu : MonoBehaviour
{

    [SerializeField] private TMP_InputField _roomName = null;

    [SerializeField] protected NetworkRunner _runner;

    [Header("Scene Settings")]
    [SerializeField] int SceneIndex = 2;

    // Attempts to start a new game session 
    public void StartShared()
    {
        StartGame(GameMode.Shared, _roomName.text);
    }

    private async void StartGame(GameMode mode, string roomName)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()

        });
    }
}