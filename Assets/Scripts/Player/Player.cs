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

    // We store the movement direction to use in FixedUpdateNetwork
    private Vector3 _moveDirection;

    private Camera _camera;

    [Networked] private float NetworkedSpeed { get; set; }
    [Networked] private int NetworkedDirection { get; set; }

    public override void Spawned()
    {
        base.Spawned();

        if (HasStateAuthority)
        {
            // Set the camera target to this player
            _camera = Camera.main;
            var topDownCameraComponent = _camera.GetComponent<TopDownCamera>();
            if (topDownCameraComponent && topDownCameraComponent.isActiveAndEnabled)
            {
                topDownCameraComponent.SetTarget(this.transform);
            }
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        // Cleanup if needed
    }

    public override void FixedUpdateNetwork()
    {
        // Only the local owner gets valid input
        if (GetInput(out NetworkInputData inputData))
        {
            Vector2 moveVec2 = inputData.moveActionValue;
            if (moveVec2.sqrMagnitude > 0.001f)
            {
                moveVec2.Normalize();

                Vector3 movement = new Vector3(moveVec2.x, moveVec2.y, 0f) * speed * Runner.DeltaTime;
                transform.position += movement;
                float currentSpeed = movement.magnitude / Runner.DeltaTime;

                // Store in [Networked] properties so everyone else gets the same values
                NetworkedSpeed = currentSpeed;
                NetworkedDirection = ComputeDirection(moveVec2);
            }
            else
            {
                NetworkedSpeed = 0f;
            }
        }

        // Now EVERY instance (local + remote) sets the Animator from the same [Networked] values
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