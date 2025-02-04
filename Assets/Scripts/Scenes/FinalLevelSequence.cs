using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevelSequence : MonoBehaviour
{
    [Header("Spaceship Settings")]
    [SerializeField] private GameObject spaceshipRenderer;
    [SerializeField] private GameObject spaceshipWithBothPlayers;
    [SerializeField] private Transform spaceshipTransform;

    [Header("Spaceship Movement")]
    [SerializeField] private float initialSpeed = 0.5f;
    [SerializeField] private float acceleration = 0.2f;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName = "NextScene"; // The name of the next scene in your build
    [SerializeField] private float timeBeforeSceneLoad = 10f;    // 10 seconds

    // Internally track if both players have entered the zone
    private bool player1InZone = false;
    private bool player2InZone = false;
    private bool sequenceStarted = false;

    // We'll store a reference to Player1 & Player2 so we can destroy or set them inactive
    private GameObject player1Obj = null;
    private GameObject player2Obj = null;

    // For moving the spaceship
    private bool isLaunching = false;
    private float currentSpeed;

    private void Start()
    {
        // Ensure thrusterParticles are stopped initially (if they aren’t already)
        currentSpeed = initialSpeed;
    }

    private void Update()
    {
        if (isLaunching)
        {
            // Move the spaceship to the right, accelerating over time
            currentSpeed += acceleration * Time.deltaTime;
            spaceshipTransform.Translate(Vector3.right * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // We check if it's a player
        if (other.CompareTag("Player"))
        {
            if (!player1InZone)
            {
                player1InZone = true;
                player1Obj = other.gameObject;
            }
            else if (!player2InZone && other.gameObject != player1Obj)
            {
                player2InZone = true;
                player2Obj = other.gameObject;
            }

            // If both players in zone and we haven't started the sequence, start it
            if (player1InZone && player2InZone && !sequenceStarted)
            {
                sequenceStarted = true;
                StartCoroutine(HandleFinalSequence());
            }
        }
    }

    /// <summary>
    /// Main coroutine to handle the final sequence events.
    /// </summary>
    private System.Collections.IEnumerator HandleFinalSequence()
    {
        Debug.Log("Both players reached the final zone. Starting final sequence...");

        // Destroy or deactivate both players
        if (player1Obj != null) Destroy(player1Obj);
        if (player2Obj != null) Destroy(player2Obj);

        // Change the spaceship sprite to show both players
        spaceshipRenderer.SetActive(false);
        spaceshipWithBothPlayers.SetActive(true);

        // Start moving the spaceship
        isLaunching = true;

        // Wait X seconds
        yield return new WaitForSeconds(timeBeforeSceneLoad);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
