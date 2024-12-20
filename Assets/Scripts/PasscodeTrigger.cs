using UnityEngine;

public class PasscodeTrigger : MonoBehaviour
{
    private bool playerInRange = false;

    // Reference to your UI panel for input
    [SerializeField] GameObject codeInputUI;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Enable the passcode UI so the player can input a code
            codeInputUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            codeInputUI.SetActive(false);
        }
    }
}
