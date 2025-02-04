using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
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
        cableOrders.Add(new string[] { "Gray", "Green", "Blue", "Purple", "Red" });
        cableOrders.Add(new string[] { "Green", "Purple", "Red", "Gray", "Blue" });
        cableOrders.Add(new string[] { "Blue", "Red", "Gray", "Purple", "Green" });
        cableOrders.Add(new string[] { "Purple", "Gray", "Green", "Red", "Blue" });
        cableOrders.Add(new string[] { "Red", "Blue", "Purple", "Green", "Gray" });
        cableOrders.Add(new string[] { "Gray", "Purple", "Red", "Blue", "Green" });
        cableOrders.Add(new string[] { "Green", "Blue", "Purple", "Red", "Gray" });
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

