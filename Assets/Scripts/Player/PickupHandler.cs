using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    GameObject pickedObject; // The object that the player is currently holding
    [SerializeField] Transform holdPosition; 

    float radius = 1.0f; // The radius in which the player can pick up objects
    
    void Update()
    {
        // If the player presses the Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (pickedObject == null)
            {
                TryPickup(); // Attempt to pick up an object
            }
            else
            {
                DropObject(); // Drop the object
            }
        }
    }

    void TryPickup()
    {
        // Search for objects within a 1.0f radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius); // Search for objects within a 1.0f radius
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PowerCell")) // If the object is a power cell
            {
                pickedObject = collider.gameObject;
                pickedObject.transform.SetParent(holdPosition); // Change the object's parent to the player's hand
                pickedObject.transform.localPosition = Vector3.zero; 
                pickedObject.GetComponent<Collider2D>().enabled = false;
                break;
            }
        }
    }

       private void DropObject()
    {
        pickedObject.transform.SetParent(null);
        pickedObject.GetComponent<Collider2D>().enabled = true; 
        pickedObject = null;
    }

}
