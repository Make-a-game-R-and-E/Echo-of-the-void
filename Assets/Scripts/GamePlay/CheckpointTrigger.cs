using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 checkpointPosition;

    private void Awake()
    {
        // If you haven't set checkpointPosition in the Inspector,
        // default to this object's position.
        if (checkpointPosition == Vector3.zero)
        {
            checkpointPosition = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object has a PlayerHealth component
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log($"Checkpoint reached! Updating checkpoint position to {checkpointPosition}");
            // In single-player, just call UpdateCheckpoint() directly
            playerHealth.UpdateCheckpoint(checkpointPosition);
        }
    }
}
