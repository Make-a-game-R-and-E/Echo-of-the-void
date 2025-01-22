using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Bar References")]
    [SerializeField] private Slider overheadHealthBar; // UI slider above the player

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    // Current health in single-player mode (no [Networked]).
    private float currentHealth;

    // Last checkpoint position
    private Vector3 lastCheckpoint;

    // Out-of-bounds damage logic
    private float damageTimer = 0f;
    private float damageInterval = 0.5f;     // Apply damage once every 0.5 second
    [SerializeField] private float outOfBoundsDamage = 5f; // e.g. 5 damage if not on the floor

    private bool isOnFloor = true;

    private void Start()
    {
        // Initialize health to max
        currentHealth = maxHealth;
        // Use the object's initial position as the first checkpoint
        lastCheckpoint = transform.position;

        // Update overhead UI
        UpdateHealthUI(currentHealth);
    }

    private void FixedUpdate()
    {
        // If we’re not on the floor, apply damage periodically
        if (!isOnFloor)
        {
            damageTimer += Time.fixedDeltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                TakeDamage(outOfBoundsDamage);
            }
        }
        else
        {
            // Reset timer if back on the floor
            damageTimer = 0f;
        }
    }

    /// <summary>
    /// Apply damage to this player.
    /// </summary>
    public void TakeDamage(float damage)
    {
        // Clamp health between 0 and max
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);

        // Update overhead health bar
        UpdateHealthUI(currentHealth);

        // Check for death
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Heal the player.
    /// </summary>
    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthUI(currentHealth);
    }

    /// <summary>
    /// Called if health falls to 0.
    /// </summary>
    private void Die()
    {
        // Respawn at the last checkpoint
        Respawn();
    }

    /// <summary>
    /// Respawn the player at the saved checkpoint.
    /// </summary>
    private void Respawn()
    {
        transform.position = lastCheckpoint;
        currentHealth = maxHealth;
        UpdateHealthUI(currentHealth);

        Debug.Log($"Player respawned at {lastCheckpoint}");
    }

    /// <summary>
    /// Updates the overhead health bar UI.
    /// </summary>
    private void UpdateHealthUI(float healthValue)
    {
        if (overheadHealthBar != null)
        {
            overheadHealthBar.value = healthValue / maxHealth;
        }
    }

    /// <summary>
    /// Updates the last checkpoint location.
    /// Call this method when the player reaches a new stage/checkpoint.
    /// </summary>
    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    /// <summary>
    /// When entering a collider tagged "Floor", we consider the player on the floor.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            isOnFloor = true;
        }
    }

    /// <summary>
    /// When exiting a collider tagged "Floor", the player is no longer on the floor.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            isOnFloor = false;
        }
    }
}
