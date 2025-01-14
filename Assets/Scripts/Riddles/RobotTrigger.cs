using Fusion;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RobotTrigger : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // The Player object must have a NetworkObject
        NetworkObject playerNetworkObject = other.GetComponent<NetworkObject>();
        if (playerNetworkObject == null) return;
        Debug.Log("Player entered trigger 12");
        // Check if this is the local player
        if (!playerNetworkObject.HasInputAuthority) return;
        Debug.Log("Player entered trigger 15");
        // If the local player has the RobotPasscodeUI script
        RobotPasscodeUI passUI = playerNetworkObject.GetComponentInChildren<RobotPasscodeUI>();
        Debug.Log("Player entered trigger 18");
        if (passUI != null)
        {
            Debug.Log("Player entered trigger 21");
            // Pass a reference to this robot’s NetworkObject
            passUI.SetRobotInRange(this.Object);
            // 'this.Object' is the RobotTrigger's NetworkObject
            Debug.Log("Player entered trigger 25");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NetworkObject playerNetworkObject = other.GetComponent<NetworkObject>();
        if (playerNetworkObject == null) return;

        // Check if this is the local player
        if (!playerNetworkObject.HasInputAuthority) return;

        // If the local player has the RobotPasscodeUI script
        RobotPasscodeUI passUI = playerNetworkObject.GetComponentInChildren<RobotPasscodeUI>();
        if (passUI != null)
        {
            // Set null to indicate no longer in range
            passUI.SetRobotInRange(null);
        }
    }
}
