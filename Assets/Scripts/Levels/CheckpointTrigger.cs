using UnityEngine;
using Fusion;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 checkpointPosition;

    private void Awake()
    {
        // If you haven't set checkpointPosition in Inspector,
        // default to this object's position
        if (checkpointPosition == Vector3.zero)
        {
            checkpointPosition = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Does the object entering have a PlayerHealth script?
        // You might need to find it in parent or child depending on your setup.
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Only the State Authority player should call the RPC to update the checkpoint.
            if (playerHealth.Object.HasStateAuthority)
            {
                Debug.Log("Checkpoint reached! Updating checkpoint position to " + checkpointPosition);
                playerHealth.UpdateCheckpointRPC(checkpointPosition);
            }
        }
    }
}
