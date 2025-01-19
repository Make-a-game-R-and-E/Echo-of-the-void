using Fusion;
using System;
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

    public void TryOpenDoor()
    {
        // Only call the RPC if the door is not open
        if (!IsOpen)
        {
            RPC_OpenDoor();
        }
        else
        {
            Debug.Log("Door is already open");
        }
    }

    // This method can be called via an RPC or directly by the Host/Server
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_OpenDoor()
    {
        // Move the robot aside
        robotTransform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, 0);

        // Disable the collider so the way is unblocked
        blockingCollider.enabled = false;

        IsOpen = true;
    }

    public bool IsDoorOpen()
    {
        return IsOpen;
    }
}
