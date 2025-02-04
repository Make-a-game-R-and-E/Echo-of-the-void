using UnityEngine;

public class InfoBox : MonoBehaviour
{
    public CellManager cellManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the player enters the trigger
            Debug.Log("Player 1 enter the cable order.");
            cellManager.SetNewOrder(); // Set a new order
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the player exits the trigger
            Debug.Log("Player 1 exut the cable order.");
            cellManager.SetNewOrder(); // Set a new order
        }
    }
}
