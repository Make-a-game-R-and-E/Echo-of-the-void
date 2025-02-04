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

    [Header("Door Sounds")]
    [Tooltip("Sound when the door opens.")]
    [SerializeField] private AudioClip doorOpenSound;

    [Tooltip("Sound when the door closes.")]
    [SerializeField] private AudioClip doorCloseSound;

    private AudioSource audioSource;
    private bool[] plateStates;
    private bool doorsCurrentlyOpen = false; // Tracks whether doors are currently open

    private void Awake()
    {
        // Initialize the plate states array
        plateStates = new bool[pressurePlates.Length];

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // 1) Update which plates are currently pressed by any "Player"
        UpdatePlateStates();

        // 2) Check if the open condition (all pressed or any pressed) is met
        bool openConditionMet = CheckDoorCondition();

        // 3) Open or close doors based on the condition
        if (openConditionMet && !doorsCurrentlyOpen)
        {
            doorsCurrentlyOpen = true;
            OpenDoors();
        }
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
    /// Opens doors by disabling them or their colliders.
    /// </summary>
    private void OpenDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                if (doorDisappearsWhenOpen)
                {
                    door.SetActive(false); // Door disappears
                }
                else
                {
                    Collider2D doorCollider = door.GetComponent<Collider2D>();
                    if (doorCollider != null) doorCollider.enabled = false; // Door becomes passable
                }
            }
        }

        // Play the open sound
        PlaySound(doorOpenSound);

        Debug.Log("Doors opened!");
    }

    /// <summary>
    /// Closes doors by re-enabling them or their colliders.
    /// </summary>
    private void CloseDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                if (doorDisappearsWhenOpen)
                {
                    door.SetActive(true); // Door reappears
                }
                else
                {
                    Collider2D doorCollider = door.GetComponent<Collider2D>();
                    if (doorCollider != null) doorCollider.enabled = true; // Door becomes solid again
                }
            }
        }

        // Play the close sound
        PlaySound(doorCloseSound);

        Debug.Log("Doors closed!");
    }

    /// <summary>
    /// Plays a sound if the clip is assigned.
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
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
