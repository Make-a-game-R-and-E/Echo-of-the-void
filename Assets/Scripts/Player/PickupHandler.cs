using UnityEngine;
using UnityEngine.InputSystem;

public class PickupHandler : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float radius = 1.0f; // The radius in which the player can pick up objects

    [Header("Input Settings")]
    [SerializeField] private InputAction pickupAction; // Assign this in the Inspector

    private GameObject pickedObject; // The object that the player is currently holding

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
            TryPickup();
        }
        else
        {
            DropObject();
        }
    }

    /// <summary>
    /// Attempt to pick up an object within the specified radius.
    /// </summary>
    private void TryPickup()
    {
        // Search for objects within the radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PowerCell"))
            {
                pickedObject = collider.gameObject;
                pickedObject.transform.SetParent(holdPosition);    // Parent to the holdPosition
                pickedObject.transform.localPosition = Vector3.zero;
                pickedObject.GetComponent<Collider2D>().enabled = false;
                break; // Stop after picking the first valid object
            }
        }
    }

    /// <summary>
    /// Drop the currently held object.
    /// </summary>
    private void DropObject()
    {
        if (pickedObject == null) return;

        pickedObject.transform.SetParent(null);
        pickedObject.GetComponent<Collider2D>().enabled = true;
        pickedObject = null;
    }
}
