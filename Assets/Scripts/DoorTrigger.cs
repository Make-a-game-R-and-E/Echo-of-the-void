using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door; // הדלת שנרצה להעלים
    [SerializeField] GameObject ball; // הכדור שהשחקן מחזיק

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball)
        {
            door.SetActive(false); // מעלים את הדלת
        }
    }
}
