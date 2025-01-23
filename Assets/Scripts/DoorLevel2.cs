using UnityEngine;

public class DoorLevel2 : MonoBehaviour
{
    [System.Serializable]
    public class DoorButtonPair
    {
        public GameObject door; // הדלת שקשורה לכפתור
        public GameObject button; // הכפתור
        [Tooltip("Custom open height for this door. Leave 0 to use the default.")]
        public float customOpenHeight = 0f; // גובה מותאם אישית לדלת
        [HideInInspector] public Vector3 originalPosition; // המיקום המקורי של הדלת
    }

    [Header("Door-Button Pairing")]
    [Tooltip("Assign each button to its corresponding door here.")]
    [SerializeField] private DoorButtonPair[] doorButtonPairs;

    [Header("Door Settings")]
    [Tooltip("Height to move the door when it opens.")]
    [SerializeField] private float openHeight = 4f;

    [Tooltip("Speed at which the door opens/closes.")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("If true, the doors will stay open after the player leaves the button.")]
    [SerializeField] private bool stayOpenOnceTriggered = false;

    private bool[] doorStates; // עוקב אחר מצב הדלתות
    private void Awake()
    {
        // ודא שמספר הדלתות שווה למספר הכפתורים
        doorStates = new bool[doorButtonPairs.Length];

        // שמור את המיקום המקורי של כל דלת
        for (int i = 0; i < doorButtonPairs.Length; i++)
        {
            if (doorButtonPairs[i].door != null)
            {
                doorButtonPairs[i].originalPosition = doorButtonPairs[i].door.transform.position;
                Debug.Log($"Original position of door {i}: {doorButtonPairs[i].originalPosition}");
            }
            else
            {
                Debug.LogWarning($"Door {i} is not assigned.");
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < doorButtonPairs.Length; i++)
        {
            if (doorButtonPairs[i].button == null) continue;

            Collider2D[] hits = Physics2D.OverlapBoxAll(
                doorButtonPairs[i].button.transform.position,
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

            if (isPressed)
            {
                if (!doorStates[i])
                {
                    OpenDoor(i);
                }
            }
            else
            {
                if (doorStates[i] && !stayOpenOnceTriggered)
                {
                    CloseDoor(i);
                }
            }
        }
    }

    private void OpenDoor(int index)
    {
        if (doorButtonPairs[index].door != null)
        {
            float heightToMove = doorButtonPairs[index].customOpenHeight > 0 ?
                                 doorButtonPairs[index].customOpenHeight :
                                 openHeight;

            StartCoroutine(MoveDoor(
                doorButtonPairs[index].door.transform,
                doorButtonPairs[index].originalPosition.y + heightToMove
            ));
            doorStates[index] = true;
        }
    }

    private void CloseDoor(int index)
    {
        if (doorButtonPairs[index].door != null)
        {
            StartCoroutine(MoveDoor(
                doorButtonPairs[index].door.transform,
                doorButtonPairs[index].originalPosition.y
            ));
            doorStates[index] = false;
        }
    }

    private System.Collections.IEnumerator MoveDoor(Transform door, float targetY)
    {
        Vector3 startPosition = door.position;
        Vector3 targetPosition = new Vector3(startPosition.x, targetY, startPosition.z);

        // בדיקה למניעת תנועה מיותרת
        if (Mathf.Approximately(door.position.y, targetY)) yield break;

        float elapsedTime = 0f;
        while (elapsedTime < moveSpeed)
        {
            door.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.position = targetPosition;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (doorButtonPairs == null) return;

        foreach (var pair in doorButtonPairs)
        {
            if (pair.button != null)
            {
                Gizmos.DrawWireCube(pair.button.transform.position, new Vector3(0.5f, 0.5f, 1f));
            }
        }
    }
}
