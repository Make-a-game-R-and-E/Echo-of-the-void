using UnityEngine;

public class RobotBlock : MonoBehaviour
{
    [Header("Door/Collider Settings")]
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private Transform robotTransform;
    [SerializeField] private float distanceToMove = 2f;

    // Local state for whether the door is open
    private bool isOpen = false;

    // Local state for whether the robot is busy (being interacted with)
    private bool busy = false;

    private void Start()
    {
        // Ensure the robot starts in the closed state
        isOpen = false;
        if (blockingCollider != null)
        {
            blockingCollider.enabled = true;
        }
    }

    /// <summary>
    /// Called by RobotPasscodeUI if the passcode is correct.
    /// </summary>
    public void TryOpenDoor()
    {
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Door is already open.");
        }
    }

    private void OpenDoor()
    {
        if (robotTransform != null)
        {
            // Move the robot aside (distanceToMove units on X-axis)
            robotTransform.position = new Vector3(
                robotTransform.position.x + distanceToMove,
                robotTransform.position.y,
                robotTransform.position.z
            );
        }

        // Disable the collider so the path is unblocked
        if (blockingCollider != null)
        {
            blockingCollider.enabled = false;
        }

        isOpen = true;
        Debug.Log("Door opened!");
    }

    /// <summary>
    /// Check if the door is already open.
    /// </summary>
    public bool IsDoorOpen()
    {
        return isOpen;
    }

    /// <summary>
    /// Returns whether the robot is currently busy (being interacted with).
    /// </summary>
    public bool IsBusy()
    {
        return busy;
    }

    /// <summary>
    /// Sets the busy state of the robot.
    /// When true, it indicates that a player is currently interacting with the robot.
    /// </summary>
    public void SetBusy(bool value)
    {
        busy = value;
    }
}
