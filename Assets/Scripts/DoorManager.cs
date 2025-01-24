using UnityEngine;

/// <summary>
/// Manages doors that open when all pressure plates are pressed.
/// </summary>
public class DoorManager : MonoBehaviour
{
    [Header("Doors to open")]
    [Tooltip("Assign all door GameObjects here.")]
    [SerializeField] private GameObject[] doors;

    [Header("Pressure plates to press")]
    [Tooltip("Assign all pressure plate GameObjects here.")]
    [SerializeField] private GameObject[] pressurePlates;

    [Header("Door Settings")]
    [Tooltip("If checked, once the doors are opened they remain open even after leaving the plates.")]
    [SerializeField] private bool stayOpenOnceTriggered = false;

    // Internal array tracking if each plate is pressed
    private bool[] plateStates;

    // Tracks whether all plates are pressed in the current frame
    private bool allPlatesPressed = false;

    private void Awake()
    {
        // Initialize the plate states array
        plateStates = new bool[pressurePlates.Length];
    }

    private void FixedUpdate()
    {
        bool localAllPressed = true;

        // Check each plate for "Player" overlaps
        for (int i = 0; i < pressurePlates.Length; i++)
        {
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

        // If all plates are pressed now, and they weren't previously, open the doors
        if (localAllPressed && !allPlatesPressed)
        {
            allPlatesPressed = true;
            OpenDoors();
        }
        // If not all pressed now, but previously were, close the doors unless they should stay open
        else if (!localAllPressed && allPlatesPressed && !stayOpenOnceTriggered)
        {
            allPlatesPressed = false;
            CloseDoors();
        }
    }

    /// <summary>
    /// Deactivate the doors (open path).
    /// </summary>
    private void OpenDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                door.SetActive(false);
            }
        }
        Debug.Log("Doors opened!");
    }

    /// <summary>
    /// Reactivate the doors (close path).
    /// </summary>
    private void CloseDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                door.SetActive(true);
            }
        }
        Debug.Log("Doors closed!");
    }

    /// <summary>
    /// Draws a visual indicator of the OverlapBox area in the Unity editor.
    /// </summary>
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
