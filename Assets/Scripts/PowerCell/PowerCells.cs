using UnityEngine;

public class PowerCell : MonoBehaviour
{
    public string cellColor; 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // If the player collides with the power cell
        {
            Debug.Log($"Collected {cellColor} power cell!");
            Destroy(gameObject); // Destroy the power cell
        }
    }
}
