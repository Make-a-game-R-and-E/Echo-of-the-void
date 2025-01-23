using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject requiredPowerCell; // הכדור הנדרש לפתיחת הדלת
    private BoxCollider doorCollider; // מאפיין את הקוליידר של הדלת
    public Vector3 originalPosition; // המיקום המקורי של הדלת
    public Vector3 newPosition; // המיקום המקורי של הדלת
    private float moveDuration = 2f; // הזמן שהדלת תישאר במקומה החדש

    void Start()
    {
        // משיג את ה-BoxCollider של הדלת
        doorCollider = GetComponent<BoxCollider>();

        // אם מצאנו את הקוליידר, מתחילים אותו במצב שלא אפשרי לעבור
        if (doorCollider != null)
        {
            doorCollider.isTrigger = false; // תחילה לא טריגר
        }

        // שומר את המיקום המקורי של הדלת
        originalPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // אם הכדור המתאים נכנס לאזור הטריגר
        if (other.CompareTag("PowerCell"))
        {
            PowerCell powerCell = other.GetComponent<PowerCell>();

            if (powerCell != null && powerCell.gameObject == requiredPowerCell)
            {
                // אם הצבע של הכדור תואם לכדור הנדרש, הופכים את הדלת ל-isTrigger
                doorCollider.isTrigger = true;
                Debug.Log("הכדור הנכון נגע בדלת. הדלת זזה!");

                // בודק את זווית הדלת כדי לקבוע לאן היא תעבור
                Vector3 targetPosition = originalPosition;

                newPosition = originalPosition + new Vector3(0, 0, 2); // המיקום החדש של הדלת
                // הזזת הדלת למיקום החדש
                StartCoroutine(MoveDoor(targetPosition));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // כאשר הכדור יוצא מאזור הטריגר, מחזירים את הדלת למצב הרגיל
        if (other.CompareTag("PowerCell"))
        {
            PowerCell powerCell = other.GetComponent<PowerCell>();

            if (powerCell != null && powerCell.gameObject == requiredPowerCell)
            {
                // אם הכדור הנכון יצא מהטריגר, מחזירים את הדלת למצב של "לא טריגר"
                doorCollider.isTrigger = false;
                Debug.Log("הכדור יצא, הדלת חזרה למצב סגור.");
            }
        }
    }

    // פונקציה להזזת הדלת למיקום החדש ואז להחזיר אותה אחרי 2 שניות
    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        // הזזת הדלת לעבר המיקום החדש
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // מחכה 2 שניות ואז מחזיר את הדלת למקום המקורי
        yield return new WaitForSeconds(2f);

        // מחזיר את הדלת למיקום המקורי
        elapsedTime = 0f;
        startingPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, originalPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
