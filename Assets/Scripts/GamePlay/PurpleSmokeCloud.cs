using UnityEngine;

/// <summary>
/// Moves a purple smoke cloud between waypoints (in random order),
/// using only cardinal (vertical/horizontal) movement.
/// Deals periodic damage to any PlayerHealth inside its trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PurpleSmokeCloud : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Waypoints that the smoke can travel to (in random order).")]
    public Transform[] waypoints;

    [Tooltip("Movement speed of the smoke cloud (units/sec).")]
    [SerializeField] private float speed = 5f;

    // Index of the current waypoint target
    private Transform currentTarget;

    // True if we're currently moving along X axis; false if we switch to Y
    private bool movingOnX = true;

    // How close we need to be to consider we have "arrived" on a particular axis
    private float arrivalThreshold = 0.5f;

    [Header("Damage Settings")]
    [Tooltip("How much damage to deal each tick.")]
    [SerializeField] private float smokeDamage = 2f;

    [Tooltip("Time (in seconds) between damage ticks when player is inside.")]
    [SerializeField] private float damageInterval = 0.1f;

    private float damageTimer = 0f;
    private bool playerInside = false;
    private PlayerHealth playerHealth; // We'll store reference when they enter

    private void Start()
    {
        // Ensure the collider is trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Pick an initial random waypoint
        PickRandomTarget();
    }

    private void Update()
    {
        if (currentTarget == null || waypoints.Length == 0) return;

        // Move the cloud in cardinal directions only
        MoveCloudCardinal();
    }

    private void MoveCloudCardinal()
    {
        Vector3 pos = transform.position;
        Vector3 targetPos = currentTarget.position;

        // 1) Move in X direction first
        if (movingOnX)
        {
            // Check if we've arrived on X
            if (Mathf.Abs(pos.x - targetPos.x) <= arrivalThreshold)
            {
                // Snap X exactly, switch to moving Y
                pos.x = targetPos.x;
                movingOnX = false;
            }
            else
            {
                // Move left or right
                float direction = (targetPos.x > pos.x) ? 1f : -1f;
                pos.x += direction * speed * Time.deltaTime;
            }
        }
        else
        {
            // 2) Move in Y direction
            if (Mathf.Abs(pos.y - targetPos.y) <= arrivalThreshold)
            {
                // Snap Y exactly
                pos.y = targetPos.y;

                // We've fully arrived at the waypoint
                transform.position = pos;

                // Pick a new random waypoint
                PickRandomTarget();

                // Next time, start moving in X again
                movingOnX = true;
                return; // End this frame’s movement
            }
            else
            {
                // Move up or down
                float direction = (targetPos.y > pos.y) ? 1f : -1f;
                pos.y += direction * speed * Time.deltaTime;
            }
        }

        // Apply the new position
        transform.position = pos;
    }

    /// <summary>
    /// Selects a random waypoint from the array that is NOT the current one.
    /// </summary>
    private void PickRandomTarget()
    {
        if (waypoints.Length == 0) return;

        Transform oldTarget = currentTarget;
        while (currentTarget == null || currentTarget == oldTarget)
        {
            int randIndex = Random.Range(0, waypoints.Length);
            currentTarget = waypoints[randIndex];
            // If there's only 1 waypoint, we won't get stuck because oldTarget==null at first
            if (waypoints.Length == 1) break;
        }
    }

    private void FixedUpdate()
    {
        // If the player is inside, apply damage periodically
        if (playerInside && playerHealth != null)
        {
            damageTimer += Time.fixedDeltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                playerHealth.TakeDamage(smokeDamage);
            }
        }
    }

    #region Trigger Handling

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If a player enters, track them for damage
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerHealth = other.GetComponent<PlayerHealth>();
            damageTimer = 0f; // reset so we start fresh
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If player leaves, stop damaging them
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealth = null;
        }
    }

    #endregion

    #region Gizmos

    // Draw lines in the editor to show the waypoints
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (waypoints != null && waypoints.Length > 0)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawSphere(waypoints[i].position, 0.1f);
                    Gizmos.DrawLine(transform.position, waypoints[i].position);
                }
            }
        }
    }

    #endregion
}
