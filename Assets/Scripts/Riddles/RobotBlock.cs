using Fusion;
using UnityEngine;

public class RobotBlock : NetworkBehaviour
{
    [SerializeField] Collider2D blockingCollider;
    [SerializeField] Transform robotTransform;
    [SerializeField] float distanceToMove = 2f;

    // We can track this if we want to avoid re-opening multiple times
    [Networked] bool IsOpen { get; set; }

    public override void Spawned()
    {
        // Make sure robot starts in the closed state
        IsOpen = false;
        blockingCollider.enabled = true;
    }

    // This method can be called via an RPC or directly by the Host/Server
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenDoor()
    {
        // Only do something if not opened yet
        if (IsOpen) return;

        // Move the robot aside
        robotTransform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, 0);

        // Disable the collider so the way is unblocked
        blockingCollider.enabled = false;

        IsOpen = true;
    }
}
