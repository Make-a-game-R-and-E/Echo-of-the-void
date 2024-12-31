using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed = 6f;

    [SerializeField] InputAction move = new InputAction(type: InputActionType.Value, expectedControlType: nameof(Vector2));


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
        Vector2 moveDirection = move.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;
        transform.position += movementVector;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Door door = other.GetComponent<Door>();
        if (door != null)
        {
            bool hasGasMask = GameManager.GetInstance().CheckHasGasMask();
            if (door.isGasRoom && !hasGasMask)
            {
                Debug.Log("You need a gas mask to enter this room!");
                UIManager uiManager = FindFirstObjectByType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowGasWarning();
                }
                return;
            }

            door.LoadRoomScene();
        }

        GasMaskItem gasMaskItem = other.GetComponent<GasMaskItem>();
        if (gasMaskItem != null)
        {
            GameManager.GetInstance().PickUpGasMask();
            Destroy(gasMaskItem.gameObject);
            Debug.Log("Gas mask acquired!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.HideGasWarning();
        }
    }
}
