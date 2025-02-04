using System.Collections.Generic;
using UnityEngine;

public class CableConnection : MonoBehaviour
{
    [SerializeField] CableManager cableManager;
    [SerializeField] GameObject door;
    List<string> playerConnections = new List<string>();

    public void ConnectCable(string cableColor)
    {
        playerConnections.Add(cableColor);
        if (playerConnections.Count == cableManager.GetCurrentOrder().Length)
        {
            CheckConnections();
        }
    }

    void CheckConnections()
    {
        string[] correctOrder = cableManager.GetCurrentOrder();
        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (playerConnections[i] != correctOrder[i])
            {
                Debug.Log("Incorrect connection! Try again.");
                playerConnections.Clear();
                return;
            }
        }

        Debug.Log("Correct connection! Door opening...");
        OpenDoor();
    }

    void OpenDoor()
    {
        // כאן נוסיף קוד לפתיחת הדלת
        // distroy(door);
    }
}

