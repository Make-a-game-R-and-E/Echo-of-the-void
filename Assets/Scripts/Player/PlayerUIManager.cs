using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiBox;
    [SerializeField] private TMP_Text messageText;

    private void Start()
    {
        uiBox.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        uiBox.SetActive(true);
        messageText.text = message;
        Invoke(nameof(HideMessage), 3f); // Hide message after 3 seconds
    }

    private void HideMessage()
    {
        uiBox.SetActive(false);
    }
}
