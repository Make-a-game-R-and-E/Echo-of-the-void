using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class DoorManager : NetworkBehaviour
{
    [Header("Doors to open")]
    [Tooltip("Assign all Door objects (GameObjects) here.")]
    [SerializeField] GameObject[] doors;

    [Header("Pressure Plates to press")]
    [Tooltip("Assign all Pressure Plate objects (GameObjects) here.")]
    [SerializeField] GameObject[] pressurePlates;

    [Header("Door Settings")]
    [Tooltip("If checked, once the doors are opened they will remain open even if the players leave the plates.")]
    [SerializeField] bool stayOpenOnceTriggered = false;

    // Internal array to track if each plate is currently pressed
    private bool[] plateStates;

    // A single networked boolean that indicates if ALL plates are pressed 
    // (i.e., puzzle is "completed" in that moment).
    // We use this for immediate door toggling.
    [Networked]
    private bool allPlatesPressed { get; set; }

    private void Awake()
    {
        plateStates = new bool[pressurePlates.Length];
    }

    // Called on every network tick (like Update, but synced via Fusion)
    public override void FixedUpdateNetwork()
    {
        bool localAllPressed = true;

        // 1) Check each plate: is there at least one "Player" on it?
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            // Use a small 2D box overlap around the plate position
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                pressurePlates[i].transform.position,
                new Vector2(0.5f, 0.5f),
                0f
            );

            bool isPressed = false;
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    isPressed = true;
                    break;
                }
            }

            plateStates[i] = isPressed;
            if (!isPressed)
            {
                localAllPressed = false;
            }
        }

        // 2) If all plates are pressed and we haven't flagged that state yet, open doors
        if (localAllPressed && !allPlatesPressed)
        {
            allPlatesPressed = true;
            RPC_OpenDoors();
        }
        // 3) If not all pressed, and previously all were pressed, close doors
        //    (unless we have marked them to stay open once triggered)
        else if (!localAllPressed && allPlatesPressed && !stayOpenOnceTriggered)
        {
            allPlatesPressed = false;
            RPC_CloseDoors();
        }
    }

    // RPC to open doors on all clients
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_OpenDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                door.SetActive(false);
            }
        }
    }

    // RPC to close doors on all clients
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_CloseDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                // Example: reactivate door (so passage is closed) or animate
                door.SetActive(true);
            }
        }
    }

    // For visual debugging in the Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (pressurePlates == null) return;

        foreach (var plate in pressurePlates)
        {
            if (plate != null)
            {
                Gizmos.DrawWireCube(plate.transform.position, new Vector3(0.5f, 0.5f, 1f));
            }
        }
    }
}
