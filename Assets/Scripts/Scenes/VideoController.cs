using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class VideoController : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField] VideoPlayer videoPlayer; // Assign this in the Inspector
    [SerializeField] string nextSceneName;    // The name of the next scene

    [Header("Input Settings")]
    [SerializeField]
    InputAction skipAction;  // Assign this in the Inspector

    private void OnEnable()
    {
        // Subscribe to the video end event
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        // Enable and subscribe to the skip action
        if (skipAction != null)
        {
            skipAction.performed += OnSkip;
            skipAction.Enable();
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the video end event
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }

        // Disable and unsubscribe from the skip action
        if (skipAction != null)
        {
            skipAction.performed -= OnSkip;
            skipAction.Disable();
        }
    }

    private void OnSkip(InputAction.CallbackContext context)
    {
        LoadNextScene();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set in the VideoController.");
        }
    }
}
