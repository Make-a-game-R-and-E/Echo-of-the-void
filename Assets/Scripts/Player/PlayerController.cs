using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed = 6f;

    // We expect a Vector2 (X, Y) from the Input System
    [SerializeField]
    InputAction move
        = new InputAction(type: InputActionType.Value,
                          expectedControlType: nameof(Vector2));

    [SerializeField] Animator animator;

    void OnEnable()
    {
        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
    }

    void FixedUpdate()
    {
        // 1. Read the movement direction from input
        Vector2 moveDirection = move.ReadValue<Vector2>();

        // 2. Calculate how far we move this frame
        Vector3 movementVector = new Vector3(
            moveDirection.x,
            moveDirection.y,
            0f
        ) * speed * Time.deltaTime;

        // 3. Update animator's Speed for Idle vs. Walking
        float currentSpeed = movementVector.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // 4. Determine which direction (Up/Right/Down/Left) we are facing
        if (currentSpeed > 0.01f)
        {
            // Compare absolute X vs absolute Y to pick direction
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                // Horizontal direction
                if (moveDirection.x > 0)
                    animator.SetInteger("Direction", 1); // Right
                else
                    animator.SetInteger("Direction", 3); // Left
            }
            else
            {
                // Vertical direction
                if (moveDirection.y > 0)
                    animator.SetInteger("Direction", 0); // Up
                else
                    animator.SetInteger("Direction", 2); // Down
            }
        }

        // 5. Move the character
        transform.position += movementVector;
    }
}
