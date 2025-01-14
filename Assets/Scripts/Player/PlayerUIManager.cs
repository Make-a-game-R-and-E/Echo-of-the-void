using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiBox;

    private void Start()
    {
        // Ensure the UI is hidden at the start
        uiBox.SetActive(false);
    }

    public void ShowUI()
    {
        uiBox.SetActive(true);
    }

    public void HideUI()
    {
        uiBox.SetActive(false);
    }
}
