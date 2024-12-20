using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeInputManager : MonoBehaviour
{
    [SerializeField] string correctPasscode = "2204"; // replace this with the code you want
    [SerializeField] TMP_InputField passcodeInputField;
    [SerializeField] GameObject blockedObject; // Assign the red block or door object
    [SerializeField] GameObject codeInputUI;

    public void CheckCode()
    {
        string enteredCode = passcodeInputField.text;
        if (enteredCode == correctPasscode)
        {
            // Correct code entered
            // Deactivate or remove the blocking object
            blockedObject.SetActive(false);

            // Optionally, hide the UI
            codeInputUI.SetActive(false);

            // You can also give feedback to the player here if needed
        }
        else
        {
            // Incorrect code, show a message or reset the input field
            passcodeInputField.text = "";
            // Perhaps show a "Try Again" message
        }
    }
}
