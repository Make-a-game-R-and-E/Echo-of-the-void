using UnityEngine;
using Fusion;

public class ButtonInteraction : NetworkBehaviour
{
    public GameObject buttonOff; // Reference to the "button_off_0" GameObject
    public GameObject buttonOn;  // Reference to the "button_on_0" GameObject

    private void Start()
    {
        // Ensure the button starts in the "off" state
        SetButtonState(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player touches the button
        {
            if (Object.HasStateAuthority) // Ensure only the authoritative client triggers the RPC
            {
                RPC_SetButtonState(true); // Trigger RPC to synchronize the button state
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player leaves the button
        {
            if (Object.HasStateAuthority) // Ensure only the authoritative client triggers the RPC
            {
                RPC_SetButtonState(false); // Trigger RPC to synchronize the button state
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SetButtonState(bool isOn)
    {
        SetButtonState(isOn); // Update the button state locally
    }

    private void SetButtonState(bool isOn)
    {
        buttonOn.SetActive(isOn);   // Show "button_on" sprite when on
        buttonOff.SetActive(!isOn); // Show "button_off" sprite when off
    }
}
