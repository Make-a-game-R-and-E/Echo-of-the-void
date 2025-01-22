using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RobotTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private RobotBlock robotBlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other is a "Player"
        if (other.CompareTag("Player"))
        {
            // Look for RobotPasscodeUI in the player
            RobotPasscodeUI passUI = other.GetComponentInChildren<RobotPasscodeUI>();
            if (passUI != null)
            {
                Debug.Log("Player entered the robot's range.");
                // Pass the robotBlock reference to the UI
                passUI.SetRobotInRange(robotBlock);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RobotPasscodeUI passUI = other.GetComponentInChildren<RobotPasscodeUI>();
            if (passUI != null)
            {
                Debug.Log("Player left the robot's range.");
                // Clear reference so the player can no longer interact
                passUI.SetRobotInRange(null);
            }
        }
    }
}
