using UnityEngine;

public class Cells : MonoBehaviour
{
        // הצבע הצפוי לכל שקע
    public string expectedColor;
    
    // הצבע שהכדור הממוקם בשקע מציג
    public string placedColor;

    // פעולה שממלאה את השקע בצבע
    public void PlaceCell(string color)
    {
        placedColor = color;
        CheckSocket();
    }

    // בודקת אם החיבור נכון
    private void CheckSocket()
    {
        if (placedColor == expectedColor)
        {
            Debug.Log("Correct placement!");
        }
        else
        {
            Debug.Log("Incorrect placement. Resetting.");
            ResetSocket();
        }
    }

    // איפוס השקע אם החיבור לא נכון
    private void ResetSocket()
    {
        placedColor = null;
        // נצטרך להחזיר את הכדור למקום ההתחלתי
    }

}

