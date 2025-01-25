using UnityEngine;

/// <summary>
/// Manages doors that open when the required pressure plates are pressed.
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

    [Tooltip("If true, ALL plates must be pressed. If false, ANY plate press will open the door.")]
    [SerializeField] private bool requireAllPlates = true;

    [Tooltip("If true, the door GameObject(s) will fully disappear (SetActive). " +
             "If false, the door remains visible but its Collider is disabled (passable).")]
    [SerializeField] private bool doorDisappearsWhenOpen = true;

    // Internal array tracking if each plate is currently pressed
    private bool[] plateStates;

    // Tracks whether the "open condition" (all/any pressed) was fulfilled last frame
    private bool doorsCurrentlyOpen = false;

    private void Awake()
    {
        // Initialize the plate states array
        plateStates = new bool[pressurePlates.Length];
    }

    private void FixedUpdate()
    {
        // 1) Update which plates are currently pressed by any "Player"
        UpdatePlateStates();

        // 2) Check if the open condition (all pressed or any pressed) is met
        bool openConditionMet = CheckDoorCondition();

        // 3) If condition is met and doors are not yet open, open them
        if (openConditionMet && !doorsCurrentlyOpen)
        {
            doorsCurrentlyOpen = true;
            OpenDoors();
        }
        // 4) If the condition is NOT met and doors were open, close them (unless they stay open once triggered)
        else if (!openConditionMet && doorsCurrentlyOpen && !stayOpenOnceTriggered)
        {
            doorsCurrentlyOpen = false;
            CloseDoors();
        }
    }

    /// <summary>
    /// Updates plateStates[i] = true if a player is overlapping that plate, otherwise false.
    /// </summary>
    private void UpdatePlateStates()
    {
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
        }
    }

    /// <summary>
    /// Checks whether we have met the condition (all pressed or any pressed) to open the doors.
    /// </summary>
    private bool CheckDoorCondition()
    {
        bool atLeastOnePressed = false;
        bool allPressed = true;

        for (int i = 0; i < plateStates.Length; i++)
        {
            if (plateStates[i])
            {
                atLeastOnePressed = true;
            }
            else
            {
                allPressed = false;
            }
        }

        // If requireAllPlates is true, we need *every* plate pressed.
        // Otherwise, having at least one plate pressed is enough.
        return requireAllPlates ? allPressed : atLeastOnePressed;
    }

    /// <summary>
    /// Deactivate (or disable the collider on) doors.
    /// </summary>
    private void OpenDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                if (doorDisappearsWhenOpen)
                {
                    // Make the door fully disappear
                    door.SetActive(false);
                }
                else
                {
                    // Keep the door visible but disable its collider so it’s passable
                    Collider2D doorCollider = door.GetComponent<Collider2D>();
                    if (doorCollider != null) doorCollider.enabled = false;
                }
            }
        }
        Debug.Log("Doors opened!");
    }

    /// <summary>
    /// Reactivate (or enable the collider on) doors.
    /// </summary>
    private void CloseDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                if (doorDisappearsWhenOpen)
                {
                    // Make the door reappear
                    door.SetActive(true);
                }
                else
                {
                    // Door is visible so just re-enable the collider
                    Collider2D doorCollider = door.GetComponent<Collider2D>();
                    if (doorCollider != null) doorCollider.enabled = true;
                }
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
