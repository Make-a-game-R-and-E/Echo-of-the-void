using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    [Header("Button Objects")]
    public GameObject buttonOff; // "button_off_0" GameObject
    public GameObject buttonOn;  // "button_on_0" GameObject

    [Header("Audio Clips")]
    public AudioClip clickSound; // Sound when button is clicked
    public AudioClip belowSound; // Sound when player gets below

    private AudioSource audioSource;
    private bool isPressed = false; // Prevents multiple trigger calls

    private void Start()
    {
        // Initialize the button in the "off" state
        SetButtonState(false);

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPressed)
        {
            // If the player enters the trigger, turn the button on and play click sound
            SetButtonState(true);
            PlaySound(clickSound);
            isPressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the player exits the trigger, turn the button off and play below sound
            SetButtonState(false);
            PlaySound(belowSound);
            isPressed = false;
        }
    }

    /// <summary>
    /// Enables the "button_on" object and disables the "button_off" object when isOn is true, and vice versa.
    /// </summary>
    private void SetButtonState(bool isOn)
    {
        if (buttonOn != null) buttonOn.SetActive(isOn);
        if (buttonOff != null) buttonOff.SetActive(!isOn);
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
