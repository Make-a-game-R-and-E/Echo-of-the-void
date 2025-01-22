using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Despawn Settings")]
    [SerializeField] private float destroyAfterSeconds = 10f; // Time after which the object despawns

    // Local timer to handle despawning
    private float despawnTimer;

    [Header("Movement Settings")]
    [SerializeField] private Vector2 initialDirection = Vector2.left; // Default movement direction
    [SerializeField] private float minSpeed = 1f;  // Minimum speed
    [SerializeField] private float maxSpeed = 5f;  // Maximum speed
    [SerializeField] private float minRotationSpeed = 45f;   // Minimum rotation speed
    [SerializeField] private float maxRotationSpeed = 180f;  // Maximum rotation speed
    [SerializeField] private float fixedDamage = 20f;        // Fixed damage to apply on collision

    // The actual movement/rotation parameters
    private Vector2 direction;
    private float speed;
    private float rotationSpeed;
    private float damage;

    private void Start()
    {
        // Initialize local properties
        InitializeProperties();

        // Set the despawn timer
        despawnTimer = destroyAfterSeconds;
    }

    private void Update()
    {
        // Countdown the despawn timer
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        // Move and rotate this object
        Vector3 movement = (Vector3)(direction * speed * Time.deltaTime);
        transform.position += movement;

        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, rotationAmount);
    }

    /// <summary>
    /// Initializes the movement properties with random values within specified ranges.
    /// </summary>
    private void InitializeProperties()
    {
        // Set the initial direction (can be customized as needed)
        direction = initialDirection.normalized;

        // Randomize speed within the specified range
        speed = Random.Range(minSpeed, maxSpeed);

        // Set fixed damage
        damage = fixedDamage;

        // Randomize rotation speed, including random sign (+/-)
        float randRotation = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationSpeed = randRotation * (Random.value > 0.5f ? 1f : -1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is a player
        if (collision.CompareTag("Player"))
        {
            // Attempt to get the PlayerHealth component
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Apply damage to the player
                playerHealth.TakeDamage(damage);

                // Destroy this object
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Collided object tagged as 'Player' does not have a PlayerHealth component.");
            }
        }
    }
}
