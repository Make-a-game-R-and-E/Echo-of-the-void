using System.Collections;
using TMPro;
using UnityEngine;

public class CodeInputManager : MonoBehaviour
{
    [SerializeField] string correctPasscode = "2204";
    [SerializeField] TMP_InputField passcodeInputField;
    [SerializeField] GameObject blockedObject;
    [SerializeField] GameObject codeInputUI;
    [SerializeField] GameObject robotBlock;
    [SerializeField] float distanceToMove = 2f;
    [SerializeField] GameObject success;
    [SerializeField] GameObject failure;
    [SerializeField] float secondsToWait = 2f;
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
            Destroy(success, secondsToWait);
        }
        else
        {
            // Incorrect code, show a message or reset the input field
            passcodeInputField.text = "";
            // Perhaps show a "Try Again" message
            failure.SetActive(true);
            StartCoroutine(HideFailureMessage());

        }
    }

    IEnumerator HideFailureMessage()
    {
        yield return new WaitForSeconds(secondsToWait);
        failure.SetActive(false);
    }
}
