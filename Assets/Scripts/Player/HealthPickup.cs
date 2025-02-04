using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Tooltip("Amount of health the player will receive upon picking this up.")]
    [SerializeField] private float healAmount = 20f;

    [Tooltip("If true, this pickup will destroy itself after healing the player.")]
    [SerializeField] private bool destroyOnUse = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding is the Player
        if (other.CompareTag("Player"))
        {
            // Attempt to get the PlayerHealth component from the collided object
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Heal the player
                playerHealth.Heal(healAmount);

                // Destroy this pickup so it can't be used again, if desired
                if (destroyOnUse)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
