using UnityEngine;

public class OpenBook : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private GameObject bookUI; // The UI element to show/hide

    [Header("Audio Clips")]
    [SerializeField] private AudioClip openSound; // Sound when UI opens
    [SerializeField] private AudioClip closeSound; // Sound when UI closes

    private AudioSource audioSource;

    private void Start()
    {
        // Ensure the UI is initially hidden
        if (bookUI != null)
            bookUI.SetActive(false);

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenUI();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CloseUI();
        }
    }

    /// <summary>
    /// Opens the UI and plays the open sound.
    /// </summary>
    private void OpenUI()
    {
        if (bookUI != null)
            bookUI.SetActive(true);

        PlaySound(openSound);
    }

    /// <summary>
    /// Closes the UI and plays the close sound.
    /// </summary>
    private void CloseUI()
    {
        if (bookUI != null)
            bookUI.SetActive(false);

        PlaySound(closeSound);
    }

    /// <summary>
    /// Plays a sound if the clip is assigned.
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
