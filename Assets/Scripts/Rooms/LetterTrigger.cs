using UnityEngine;

public class LetterTrigger : MonoBehaviour
{

    [SerializeField] GameObject LetterUI;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LetterUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LetterUI.SetActive(false);
        }
    }
}
