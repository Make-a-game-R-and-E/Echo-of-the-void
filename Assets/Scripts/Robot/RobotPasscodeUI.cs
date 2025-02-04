using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotPasscodeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject passcodePanel;
    [SerializeField] private TMP_InputField passcodeInputField;
    [SerializeField] private GameObject successMessage;
    [SerializeField] private GameObject failureMessage;
    [SerializeField] private TMP_Text statusText; // Text area for status messages

    [Header("Settings")]
    [SerializeField] private string correctPasscode = "2204"; // Single correct passcode
    [SerializeField] private float secondsToWait = 2f;

    [Header("Input")]
    [SerializeField]
    private InputAction interactAction = new InputAction("Interact", InputActionType.Button);

    [Header("Player Reference")]
    [Tooltip("Assign the PlayerMovement script in the Inspector.")]
    [SerializeField] private PlayerMovement playerMovement;

    private RobotBlock currentRobotBlock; // Reference to the nearby robot's RobotBlock
    private bool canEnterPasscode;        // True if player is within range of a robot

    private void OnEnable()
    {
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

    private void Start()
    {
        // Initialize UI elements
        passcodePanel.SetActive(false);
        successMessage.SetActive(false);
        failureMessage.SetActive(false);
        statusText.text = ""; // Clear the status text at startup
    }

    // Called when the interact button is pressed
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (canEnterPasscode)
        {
            // If the robot is already in use by another player, show the busy message
            if (currentRobotBlock != null && currentRobotBlock.IsBusy())
            {
                StartCoroutine(ShowBusyMessage());
                return;
            }

            // If the door is already open, no need to interact
            if (currentRobotBlock && currentRobotBlock.IsDoorOpen())
            {
                Debug.Log("The door is already open, no need to show the panel.");
                return;
            }
            TogglePanel();
        }
    }

    /// <summary>
    /// Coroutine that displays a busy message in the status text area for a short time.
    /// </summary>
    private IEnumerator ShowBusyMessage()
    {
        statusText.text = "Robot is busy.";
        yield return new WaitForSeconds(secondsToWait);
        statusText.text = "";
    }

    /// <summary>
    /// Toggles the passcode panel on/off, updates the player's ability to move, 
    /// and sets the robot's busy state.
    /// </summary>
    public void TogglePanel()
    {
        bool newState = !passcodePanel.activeSelf;
        passcodePanel.SetActive(newState);

        // Mark the robot as busy when the panel opens, and free it when closed.
        if (currentRobotBlock != null)
        {
            currentRobotBlock.SetBusy(newState);
        }

        if (newState)
        {
            // Panel just opened:
            // Reset input and messages, disable player movement
            passcodeInputField.text = "0000";
            successMessage.SetActive(false);
            failureMessage.SetActive(false);
            if (playerMovement != null)
                playerMovement.canMove = false;
        }
        else
        {
            // Panel just closed:
            if (playerMovement != null)
                playerMovement.canMove = true;
        }
    }

    /// <summary>
    /// Called by the UI "Submit" button to check the passcode.
    /// </summary>
    public void OnSubmitPasscode()
    {
        if (passcodeInputField.text == correctPasscode)
        {
            // Show success message
            successMessage.SetActive(true);
            Destroy(successMessage, secondsToWait);

            // Open the door on the nearby robot
            OpenRobotDoor();

            // Hide the panel (which also re-enables player movement and resets busy state)
            HidePanel();
        }
        else
        {
            // Show failure message
            failureMessage.SetActive(true);
            passcodeInputField.text = "";
            StartCoroutine(HideFailureMessage());
        }
    }

    private IEnumerator HideFailureMessage()
    {
        yield return new WaitForSeconds(secondsToWait);
        failureMessage.SetActive(false);
    }

    private void HidePanel()
    {
        passcodePanel.SetActive(false);

        // Free the robot if the panel is closed.
        if (currentRobotBlock != null)
            currentRobotBlock.SetBusy(false);

        if (playerMovement != null)
            playerMovement.canMove = true;
    }

    /// <summary>
    /// Attempt to open the robot door (if we have a reference).
    /// </summary>
    private void OpenRobotDoor()
    {
        if (currentRobotBlock)
        {
            currentRobotBlock.TryOpenDoor();
        }
        else
        {
            Debug.LogWarning("No RobotBlock reference found to open door.");
        }
    }

    /// <summary>
    /// Sets which robot the player is in range of.
    /// Passing 'null' means no robot in range.
    /// </summary>
    public void SetRobotInRange(RobotBlock robotBlock)
    {
        currentRobotBlock = robotBlock;
        canEnterPasscode = (robotBlock != null);
    }

    /// <summary>
    /// Returns true if the passcode panel is open.
    /// </summary>
    public bool IsPanelOpen => passcodePanel.activeSelf;

    /// <summary>
    /// Closes the panel via the close button (if you have one).
    /// </summary>
    public void OnClickClose()
    {
        HidePanel();
    }
}
