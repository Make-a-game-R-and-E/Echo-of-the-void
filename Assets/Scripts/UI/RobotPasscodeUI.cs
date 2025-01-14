using Fusion;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class RobotPasscodeUI : MonoBehaviour
{
    [SerializeField] private GameObject passcodePanel;
    [SerializeField] private TMP_InputField passcodeInputField;
    [SerializeField] private GameObject successMessage;
    [SerializeField] private GameObject failureMessage;

    // If you want a single correct passcode for everyone
    [SerializeField] private string correctPasscode = "2204";

    [SerializeField] private NetworkObject robotNetworkObject;

    // We'll keep track if we are near the robot
    private bool canEnterPasscode;

    // Reference to runner if we need to call RPC
    private NetworkRunner _runner;

    [SerializeField] float secondsToWait = 2f;

    [SerializeField] private InputAction interactAction = new InputAction("Interact", InputActionType.Button, "<Keyboard>/q");

    private void OnEnable()
    {
        // 2) Make sure the action is enabled
        if (interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed += OnInteractPerformed;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.performed -= OnInteractPerformed;
            interactAction.Disable();
        }
    }

    // 3) Callback method for when the 'Interact' action is performed
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (canEnterPasscode)
        {
            TogglePanel();
        }
    }
    private void Start()
    {
        passcodePanel.SetActive(false);
        successMessage.SetActive(false);
        failureMessage.SetActive(false);

        _runner = FindFirstObjectByType<NetworkRunner>();
    }

    public void TogglePanel()
    {
        passcodePanel.SetActive(!passcodePanel.activeSelf);
        if (passcodePanel.activeSelf)
        {
            // Reset any old messages/input
            passcodeInputField.text = "0000";
            successMessage.SetActive(false);
            failureMessage.SetActive(false);
        }
    }

    // Called by the UI button
    public void OnSubmitPasscode()
    {
        if (passcodeInputField.text == correctPasscode)
        {
            // Show success UI
            successMessage.SetActive(true);
            Destroy(successMessage, secondsToWait);

            // Do the actual robot opening logic
            OpenRobotDoor();

            // Hide panel after success
            HidePanel();
        }
        else
        {

            // Show failure message
            failureMessage.SetActive(true);
            // Clear input
            passcodeInputField.text = "";
            // hide the message after some time
            StartCoroutine(HideFailureMessage());
        }
    }
    IEnumerator HideFailureMessage()
    {
        yield return new WaitForSeconds(secondsToWait);
        failureMessage.SetActive(false);
    }

    private void HidePanel()
    {
        passcodePanel.SetActive(false);
    }

    // This is where we call the Robot's RPC
    private void OpenRobotDoor()
    {
        Debug.Log("Opening door_ 120");
        if (robotNetworkObject != null)
        {
            Debug.Log("Opening door_ 122");
            // We fetch the RobotBlock script from the network object
            RobotBlock robotBlock = robotNetworkObject.GetComponent<RobotBlock>();
            if (robotBlock != null)
            {
                Debug.Log("Opening door_ 127");
                // Call the RPC on the server to open the door
                robotBlock.RPC_OpenDoor();
            }
            else
            {
                Debug.LogWarning("RobotBlock component not found on robot object");
            }
        }
    }

    // Called by Trigger/Collider logic
    public void SetRobotInRange(NetworkObject robot)
    {
        robotNetworkObject = robot;
        canEnterPasscode = (robot != null);
    }
    public bool IsPanelOpen
    {
        get { return passcodePanel.activeSelf; }
    }

    // Optionally, a new "X" button in the UI calls this
    public void OnClickClose()
    {
        HidePanel();
    }

}
