using UnityEngine;

public class PowerCell : MonoBehaviour
{
    [SerializeField] string cellColor; // הצבע של הכדור

    public string GetCellColor()
    {
        return cellColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // אם השחקן פוגע בכדור
        {
            Debug.Log($"Collected {cellColor} power cell!");
            Destroy(gameObject); // השמדת הכדור
        }
    }
}