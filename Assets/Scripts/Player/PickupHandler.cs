using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupHandler : NetworkBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdPosition;
    [SerializeField] float radius = 1.0f; // The radius in which the player can pick up objects

    [Header("Input Settings")]
    [SerializeField] InputAction pickupAction; // Assign this in the Inspector

    GameObject pickedObject; // The object that the player is currently holding


    private void OnEnable()
    {
        if (pickupAction != null)
        {
            pickupAction.Enable();
            pickupAction.performed += OnPickupAction;
        }
    }

    private void OnDisable()
    {
        if (pickupAction != null)
        {
            pickupAction.performed -= OnPickupAction;
            pickupAction.Disable();
        }
    }

    private void OnPickupAction(InputAction.CallbackContext context)
    {
        if (pickedObject == null)
        {
            TryPickup_RPC();
        }
        else
        {
            DropObject_RPC();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void TryPickup_RPC()
    {
        // Search for objects within a 1.0f radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius); // Search for objects within a 1.0f radius
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PowerCell")) // If the object is a power cell
            {
                pickedObject = collider.gameObject;
                pickedObject.transform.SetParent(holdPosition); // Change the object's parent to the player's hand
                pickedObject.transform.localPosition = Vector3.zero;
                pickedObject.GetComponent<Collider2D>().enabled = false;
                break;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void DropObject_RPC()
    {
        pickedObject.transform.SetParent(null);
        pickedObject.GetComponent<Collider2D>().enabled = true;
        pickedObject = null;
    }
}
