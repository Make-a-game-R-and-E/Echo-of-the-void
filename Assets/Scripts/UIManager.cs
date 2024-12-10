using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject warningPanel;


    public void ShowGasWarning()
    {
        warningPanel.SetActive(true);
    }

    public void HideGasWarning()
    {
        warningPanel.SetActive(false);
    }
}
