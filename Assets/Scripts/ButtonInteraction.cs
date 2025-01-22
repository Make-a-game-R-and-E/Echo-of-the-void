using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    [Header("Button Objects")]
    public GameObject buttonOff; // "button_off_0" GameObject
    public GameObject buttonOn;  // "button_on_0" GameObject

    private void Start()
    {
        // Initialize the button in the "off" state
        SetButtonState(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the player enters the trigger, turn the button on
            SetButtonState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the player exits the trigger, turn the button off
            SetButtonState(false);
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
}
