using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer; // Assign this in the Inspector
    [SerializeField] private string videoFileName = "MyVideo.mp4"; // Name of the video file in StreamingAssets

    void Start()
    {
        // Set the video URL dynamically based on the platform
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

#if UNITY_WEBGL
        videoPlayer.url = videoPath; // WebGL uses a direct URL
        Debug.Log("WebGL Video Path: " + videoPath);
#else
        videoPlayer.url = "file://" + videoPath; // Local platforms use an absolute file path
        Debug.Log("Local Video Path: " + videoPlayer.url);
#endif

        videoPlayer.Prepare();

        // Automatically play the video when ready
        videoPlayer.prepareCompleted += (vp) =>
        {
            PlayVideo();
            Debug.Log("Video started playing.");
        };
    }

    /// Plays the video and makes the video UI visible.
    public void PlayVideo()
    {

        if (!videoPlayer.enabled)
        {
            videoPlayer.enabled = true;
        }

        videoPlayer.Play();
    }
}
