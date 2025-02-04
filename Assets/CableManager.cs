using System.Collections.Generic;
using UnityEngine;

public class CableManager : MonoBehaviour
{
    public List<string[]> cableOrders = new List<string[]>(); // רשימה של סדרי חיבורים אפשריים
    private string[] currentOrder; // הסדר הנוכחי שהשחקן רואה

    private void Start()
    {
        GenerateCableOrders(); // יצירת הסידורים האפשריים
        SetNewOrder(); // הגדרת סידור ראשון
    }

    void GenerateCableOrders()
    {
        cableOrders.Add(new string[] { "Red -> Blue", "Green -> Yellow", "Blue -> Green", "Yellow -> Red" });
        cableOrders.Add(new string[] { "Red -> Yellow", "Green -> Blue", "Blue -> Red", "Yellow -> Green" });
        cableOrders.Add(new string[] { "Blue -> Green", "Green -> Blue", "Red -> Yellow", "Yellow -> Red" });
        cableOrders.Add(new string[] { "Blue -> Yellow", "Green -> Red", "Red -> Green", "Yellow -> Blue" });
        cableOrders.Add(new string[] { "Yellow -> Red", "Green -> Blue", "Blue -> Green", "Red -> Yellow" });
        cableOrders.Add(new string[] { "Yellow -> Blue", "Green -> Red", "Blue -> Yellow", "Red -> Green" });
        cableOrders.Add(new string[] { "Green -> Yellow", "Red -> Blue", "Blue -> Red", "Yellow -> Green" });
        cableOrders.Add(new string[] { "Green -> Red", "Red -> Green", "Blue -> Yellow", "Yellow -> Blue" });
    }

      public void SetNewOrder()
    {
        ShuffleOrders(); // ערבוב לפני בחירה
        int randomIndex = Random.Range(0, cableOrders.Count);
        currentOrder = cableOrders[randomIndex];
        Debug.Log("New Cable Order: " + string.Join(", ", currentOrder));
    }

    void ShuffleOrders()
    {
        for (int i = cableOrders.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (cableOrders[i], cableOrders[j]) = (cableOrders[j], cableOrders[i]); // חילוף מקומות
        }
    }
    public string[] GetCurrentOrder()
    {
        return currentOrder;
    }
}

