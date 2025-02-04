using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CellSocket : MonoBehaviour
{
    [Header("Which color is required here?")]
    public CellColor requiredColor;

    // The power cell that is currently placed in this socket (if any)
    public PowerCell placedCell;

    private Collider2D _collider;

    private void Awake()
    {
        // Make sure this socket’s collider is set as a trigger
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If something enters the socket area
        if (placedCell != null) return; // Already occupied

        if (other.CompareTag("PowerCell"))
        {
            // The player must have just dropped a PowerCell
            var cell = other.GetComponent<PowerCell>();
            if (cell == null) return;

            // Check if the cell’s color matches what we need
            if (cell.cellColor == requiredColor)
            {
                // It's the correct color: we "lock" it into this socket
                placedCell = cell;

                // snap the cell’s position to the socket:
                cell.transform.position = transform.position;
                cell.transform.SetParent(transform);

                // We can disable the collider or make the object kinematic so
                // it no longer moves:
                var cellCollider = cell.GetComponent<Collider2D>();
                if (cellCollider) cellCollider.enabled = false;

                // fully remove it from the player's "PickupHandler":
                var pickupHandler = cell.GetComponentInParent<PickupHandler>();
                if (pickupHandler && pickupHandler.pickedObject == cell.gameObject)
                {
                    pickupHandler.DropObject();
                }

                var sr = this.GetComponent<SpriteRenderer>();
                sr.color = Color.white; // Make it look like it's locked in place

                Debug.Log("Correct cell placed!");
            }
            else
            {
                Debug.Log("Wrong color cell for this socket!");
            }
        }
    }
}
