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

    [Header("Settings")]
    [SerializeField] private string correctPasscode = "2204"; // Single correct passcode
    [SerializeField] private float secondsToWait = 2f;

    [Header("Input")]
    [SerializeField]
    private InputAction interactAction =
        new InputAction("Interact", InputActionType.Button);

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
        // Initialize UI
        passcodePanel.SetActive(false);
        successMessage.SetActive(false);
        failureMessage.SetActive(false);
    }

    // Called when the interact button is pressed
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (canEnterPasscode)
        {
            // If the door is already open, don't show the panel
            if (currentRobotBlock && currentRobotBlock.IsDoorOpen())
            {
                Debug.Log("The door is already open, no need to show the panel.");
                return;
            }
            TogglePanel();
        }
    }

    /// <summary>
    /// Toggles the passcode panel on/off and updates player movement availability.
    /// </summary>
    public void TogglePanel()
    {
        bool newState = !passcodePanel.activeSelf;
        passcodePanel.SetActive(newState);

        if (newState)
        {
            // Panel just opened:
            // - Reset any old messages/input
            passcodeInputField.text = "0000";
            successMessage.SetActive(false);
            failureMessage.SetActive(false);

            // Disable player movement
            if (playerMovement != null)
                playerMovement.canMove = false;
        }
        else
        {
            // Panel just closed:
            // - Enable player movement again
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

            // Hide the panel (which also re-enables player movement)
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

        // Re-enable player movement
        if (playerMovement != null)
            playerMovement.canMove = true;
    }

    /// <summary>
    /// Attempt to open the robot door (if we have a reference to one).
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
    /// Set which robot the player is in range of.
    /// Passing 'null' means no robot in range.
    /// </summary>
    public void SetRobotInRange(RobotBlock robotBlock)
    {
        currentRobotBlock = robotBlock;
        canEnterPasscode = (robotBlock != null);
    }

    /// <summary>
    /// Helper property if other scripts need to check if the passcode panel is open.
    /// </summary>
    public bool IsPanelOpen
    {
        get { return passcodePanel.activeSelf; }
    }

    /// <summary>
    /// If the UI has a close button, you can wire this up.
    /// </summary>
    public void OnClickClose()
    {
        HidePanel();
    }
}
