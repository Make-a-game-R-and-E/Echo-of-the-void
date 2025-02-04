using UnityEngine;

public class ColorDoorManager : MonoBehaviour
{
    [Header("Doors to open")]
    [SerializeField] private GameObject[] doors;

    [Header("Pressure plates to press")]
    [SerializeField] private GameObject[] pressurePlates;

    [Header("Door Settings")]
    [SerializeField] private bool stayOpenOnceTriggered = false;
    [SerializeField] private bool requireAllPlates = true;
    [SerializeField] private bool doorDisappearsWhenOpen = true;

    [Header("Required Ball to Open")]
    [Tooltip("The specific ball object needed to open the door.")]
    [SerializeField] private GameObject requiredBall;  // הכדור שצריך כדי לפתוח את הדלת

    private bool[] plateStates;
    private bool doorsCurrentlyOpen = false;

    private void Awake()
    {
        plateStates = new bool[pressurePlates.Length];
    }

    private void FixedUpdate()
    {
        UpdatePlateStates();

        bool openConditionMet = CheckDoorCondition();

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

    private void UpdatePlateStates()
    {
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(pressurePlates[i].transform.position, new Vector2(0.5f, 0.5f), 0f);

            bool isPressed = false;
            foreach (var hit in hits)
            {
                if (hit.gameObject == requiredBall) // אם הכדור הנדרש נמצא על הלחצן
                {
                    isPressed = true;
                    break;
                }
            }

            plateStates[i] = isPressed;
        }
    }

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

        return requireAllPlates ? allPressed : atLeastOnePressed;
    }

    private void OpenDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                if (doorDisappearsWhenOpen)
                {
                    door.SetActive(false);
                }
                else
                {
                    Collider2D doorCollider = door.GetComponent<Collider2D>();
                    if (doorCollider != null) doorCollider.enabled = false;
                }
            }
        }
        Debug.Log("Doors opened!");
    }

    private void CloseDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                if (doorDisappearsWhenOpen)
                {
                    door.SetActive(true);
                }
                else
                {
                    Collider2D doorCollider = door.GetComponent<Collider2D>();
                    if (doorCollider != null) doorCollider.enabled = true;
                }
            }
        }
        Debug.Log("Doors closed!");
    }
}
