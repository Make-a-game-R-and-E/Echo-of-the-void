using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            door.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exited");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger");
            door.SetActive(true);
        }
    }
}

