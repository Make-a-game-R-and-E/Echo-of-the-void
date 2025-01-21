using Fusion;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkObject))]
public class PlayerHealth : NetworkBehaviour
{
    [Header("Health Bar References")]
    [SerializeField] private Slider overheadHealthBar;  // World-space UI slider above the player

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    // Networked property to synchronize health across all clients.
    [Networked]
    [SerializeField] private float CurrentHealth { get; set; }

    // Last checkpoint position (also networked)
    [Networked]
    private Vector3 LastCheckpoint { get; set; }

    // Timer and settings for out-of-bounds damage
    private float damageTimer = 0f;
    private float damageInterval = 0.5f;       // Apply damage once every 0.5 second
    [SerializeField] private float outOfBoundsDamage = 5f;   // 5 damage if not on the floor

    private bool isOnFloor = true;

    // Called when this object is created/spawned on the network
    public override void Spawned()
    {
        // Only the State Authority initializes the health and the checkpoint
        if (Object.HasStateAuthority)
        {
            CurrentHealth = maxHealth;
            LastCheckpoint = transform.position;
        }

        // Sync UI to the current health on spawn
        UpdateHealthUI(CurrentHealth);
    }

    public override void FixedUpdateNetwork()
    {
        // Only the State Authority applies out-of-bounds damage
        if (Object.HasStateAuthority)
        {
            bool onFloorCheck = CheckIfOnFloor();

            if (!onFloorCheck)
            {
                damageTimer += Runner.DeltaTime;
                if (damageTimer >= damageInterval)
                {
                    damageTimer = 0f;
                    // Apply out-of-bounds damage via RPC
                    TakeDamageRPC(outOfBoundsDamage);
                }
            }
            else
            {
                damageTimer = 0f;
            }
        }
    }

    /// Call this RPC to apply damage to the player (called by the State Authority).
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void TakeDamageRPC(float damage)
    {
        // Clamp the new health
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, maxHealth);

        // Update UI for all clients
        UpdateHealthUI(CurrentHealth);

        // Only the State Authority checks for death
        if (Object.HasStateAuthority && CurrentHealth <= 0f)
        {
            Die();
        }
    }

    /// Call this RPC to heal the player (called by the State Authority).
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void HealRPC(float amount)
    {
        // Clamp the new health
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, maxHealth);

        // Update UI for all clients
        UpdateHealthUI(CurrentHealth);
    }

    /// Called only by the State Authority when CurrentHealth reaches 0 or below.
    private void Die()
    {
        // Respawn via an RPC (which will run on all clients).
        RespawnRPC();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RespawnRPC()
    {
        // Only the State Authority modifies the authoritative position
        if (Object.HasStateAuthority)
        {
            // Get the Player (movement) script on the same object
            Player playerMovement = GetComponent<Player>();
            if (playerMovement != null)
            {
                // Set both NetworkedPosition and transform.position to LastCheckpoint
                playerMovement.NetworkedPosition = LastCheckpoint;
                transform.position = LastCheckpoint;
            }

            // Restore health to max
            CurrentHealth = maxHealth;

            Debug.Log("Player respawned at " + LastCheckpoint);
        }

        // Update UI immediately after respawning for all clients
        UpdateHealthUI(CurrentHealth);
    }

    /// Update the UI elements (overhead bar and personal UI bar).
    private void UpdateHealthUI(float healthValue)
    {
        float normalizedHealth = healthValue / maxHealth;

        // Overhead Health Bar (for everyone to see)
        if (overheadHealthBar != null)
        {
            overheadHealthBar.value = normalizedHealth;
        }
    }

    /// Update the checkpoint (called by the State Authority when the player reaches a new stage).
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void UpdateCheckpointRPC(Vector3 newCheckpoint)
    {
        LastCheckpoint = newCheckpoint;
    }

    // Called when the player enters a trigger marked "Floor"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            isOnFloor = true;
        }
    }

    // Called when the player exits a trigger marked "Floor"
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            isOnFloor = false;
        }
    }

    // This method is used to decide out-of-bounds damage
    private bool CheckIfOnFloor()
    {
        return isOnFloor;
    }
}
