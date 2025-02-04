using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RobotTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RobotBlock robotBlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is tagged "Player"
        if (other.CompareTag("Player"))
        {
            // Find the RobotPasscodeUI component on the player
            RobotPasscodeUI passUI = other.GetComponentInChildren<RobotPasscodeUI>();
            if (passUI != null)
            {
                Debug.Log("Player entered the robot's range.");
                // Provide the robotBlock reference so the UI can interact with the robot.
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
                // Clear the reference so the player can no longer interact with the robot.
                passUI.SetRobotInRange(null);
            }
        }
    }
}
