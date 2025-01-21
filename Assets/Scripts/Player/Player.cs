using Fusion;
using UnityEngine;

// Ensure there's a NetworkTransform on this object
[RequireComponent(typeof(NetworkTransform))]
public class Player : NetworkBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] private float speed = 6f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    // Reference to the RobotPasscodeUI (if you have an in-game UI that can block movement)
    [SerializeField] private RobotPasscodeUI _passcodeUI;

    // For animation only: how fast the player is moving
    [Networked] private float NetworkedSpeed { get; set; }
    // For animation only: 0=Up,1=Right,2=Down,3=Left
    [Networked] private int NetworkedDirection { get; set; }

    // Networked property that holds the authoritative position
    [Networked]
    public Vector3 NetworkedPosition { get; set; }

    private Camera _camera;

    public override void Spawned()
    {
        base.Spawned();

        // If we are the State Authority for this object, initialize the NetworkedPosition
        if (Object.HasStateAuthority)
        {
            NetworkedPosition = transform.position;
        }

        // If we have Input Authority (we control this player), set up our camera, etc.
        if (HasInputAuthority)
        {
            _camera = Camera.main;
            var topDownCamera = _camera?.GetComponent<TopDownCamera>();
            if (topDownCamera && topDownCamera.isActiveAndEnabled)
            {
                topDownCamera.SetTarget(this.transform);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        // --- AUTHORITY SIDE ---
        if (Object.HasStateAuthority)
        {
            if (GetInput(out NetworkInputData inputData))
            {
                bool canMove = true;
                if (_passcodeUI != null && _passcodeUI.IsPanelOpen)
                {
                    canMove = false;
                }

                if (canMove)
                {
                    Vector2 moveVec2 = inputData.moveActionValue;
                    if (moveVec2.sqrMagnitude > 0.001f)
                    {
                        moveVec2.Normalize();

                        // Calculate movement
                        Vector3 movement = new Vector3(moveVec2.x, moveVec2.y, 0f) * speed * Runner.DeltaTime;
                        transform.position += movement;

                        // Update authoritative position
                        NetworkedPosition = transform.position;

                        // Calculate speed & direction for animation
                        float currentSpeed = movement.magnitude / Runner.DeltaTime;
                        NetworkedSpeed = currentSpeed;
                        NetworkedDirection = ComputeDirection(moveVec2);
                    }
                    else
                    {
                        NetworkedSpeed = 0f;
                    }
                }
            }
        }
        else
        {
            // --- NON-AUTHORITY CLIENTS ---
            // Follow the authoritative NetworkedPosition
            transform.position = NetworkedPosition;
        }

        // --- ALL INSTANCES ---
        // Update the animator parameters using the networked values
        animator.SetFloat("Speed", NetworkedSpeed);
        animator.SetInteger("Direction", NetworkedDirection);
    }

    private int ComputeDirection(Vector2 moveVec2)
    {
        // 0=Up,1=Right,2=Down,3=Left
        if (Mathf.Abs(moveVec2.x) > Mathf.Abs(moveVec2.y))
        {
            // Horizontal
            return moveVec2.x > 0 ? 1 : 3;
        }
        else
        {
            // Vertical
            return moveVec2.y > 0 ? 0 : 2;
        }
    }
}
