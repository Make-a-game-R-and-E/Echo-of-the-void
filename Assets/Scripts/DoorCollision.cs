using UnityEngine;

public class DoorCollision : MonoBehaviour
{
    public GameObject linkedBall; // הכדור שקשור לדלת
    private bool ballReturned = false; // אם הכדור הוחזר לדלת
    private Collider doorCollider;

    void Start()
    {
        // שמירת ה-Collider של הדלת כדי שנוכל לשלוט עליו
        doorCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // אם השחקן נוגע בדלת
        if (other.CompareTag("Player"))
        {
            // אם הכדור הוחזר, ניתן לשחקן לעבור
            if (ballReturned)
            {
                Debug.Log("You can pass through the door!");
                // למחוק את הדלת או להזיז אותה
                Destroy(gameObject); // לדוגמה, מחיקת הדלת אחרי שהשחקן עבר
            }
            else
            {
                // השחקן לא יכול לעבור אם הכדור לא הוחזר
                Debug.Log("You need to return the ball to pass.");
                // נוודא שהשחקן לא יכול לעבור דרך הדלת
                doorCollider.enabled = true; // הדלת סגורה
            }
        }
    }

    // פונקציה שמתעדכנת כשכדור פוגע בדלת
    public void OnBallReturn()
    {
        // עדכון הדגל לכך שהכדור הוחזר לדלת
        ballReturned = true;
        Debug.Log("Ball returned to the door!");
        // כשכדור הוחזר, נבצע פעולה שתאפשר את המעבר
        doorCollider.enabled = false; // מכבים את ה-Collider של הדלת, כך שהשחקן יוכל לעבור
    }

    // אפשר גם להחזיר את הדלת למצב סגור אם צריך
    public void ResetDoor()
    {
        ballReturned = false;
        doorCollider.enabled = true; // מחזירים את ה-Collider על מנת למנוע מעבר
    }
}
