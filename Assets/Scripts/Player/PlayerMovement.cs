using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] private float speed = 6f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    // New Input System: a Vector2 input action for movement
    [SerializeField]
    private InputAction move = new InputAction(
        type: InputActionType.Value,
        expectedControlType: nameof(Vector2)
    );

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float currentSpeed;
    private int currentDirection;

    // Public so RobotPasscodeUI can directly set canMove = false/true
    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Update()
    {
        // If canMove is false, zero out input
        if (!canMove)
        {
            movementInput = Vector2.zero;
            return;
        }

        // Otherwise, read the movement input as normal
        movementInput = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Apply movement via Rigidbody velocity
        rb.linearVelocity = movementInput * speed;

        // Update animation based on the Rigidbody’s current velocity
        currentSpeed = rb.linearVelocity.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // If moving, compute direction
        if (currentSpeed > 0.01f)
        {
            currentDirection = ComputeDirection(rb.linearVelocity.normalized);
            animator.SetInteger("Direction", currentDirection);
        }
        else
        {
            // If not moving, ensure Speed is zero
            animator.SetFloat("Speed", 0f);
        }
    }

    /// <summary>
    /// 0=Up, 1=Right, 2=Down, 3=Left
    /// Determines direction based on the normalized movement vector.
    /// </summary>
    private int ComputeDirection(Vector2 moveVec2)
    {
        // Horizontal movement dominates if its absolute value is greater
        if (Mathf.Abs(moveVec2.x) > Mathf.Abs(moveVec2.y))
        {
            // 1=Right, 3=Left
            return moveVec2.x > 0 ? 1 : 3;
        }
        else
        {
            // 0=Up, 2=Down
            return moveVec2.y > 0 ? 0 : 2;
        }
    }
}
