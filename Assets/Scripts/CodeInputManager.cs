using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeInputManager : MonoBehaviour
{
    [SerializeField] string correctPasscode = "2204"; // replace this with the code you want
    [SerializeField] TMP_InputField passcodeInputField;
    [SerializeField] GameObject blockedObject; // Assign the red block or door object
    [SerializeField] GameObject codeInputUI;
    [SerializeField] GameObject robotBlock;
    [SerializeField] float distanceToMove = 2f;
    [SerializeField] GameObject success;
    [SerializeField] GameObject failure;
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

            // move the robot to the side
            robotBlock.transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);

            // You can also give feedback to the player here if needed
            failure.SetActive(false); // Hide the failure message if it was shown before
            success.SetActive(true);
            Destroy(success, 2f);
        }
        else
        {
            // Incorrect code, show a message or reset the input field
            passcodeInputField.text = "";
            // Perhaps show a "Try Again" message
            failure.SetActive(true);
            Destroy(failure, 2f);
        }
    }
}
