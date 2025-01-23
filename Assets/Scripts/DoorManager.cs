// using UnityEngine;

// public class DoorManager : MonoBehaviour
// {
//     [Header("Doors to open")]
//     [Tooltip("Assign all Door objects (GameObjects) here.")]
//     [SerializeField] private GameObject[] doors;

//     [Header("Pressure Plates to press")]
//     [Tooltip("Assign all Pressure Plate objects (GameObjects) here.")]
//     [SerializeField] private GameObject[] pressurePlates;

//     [Header("Door Settings")]
//     [Tooltip("If checked, once the doors are opened they will remain open even if the players leave the plates.")]
//     [SerializeField] private bool stayOpenOnceTriggered = false;

//     // Internal array to track if each plate is currently pressed
//     private bool[] plateStates;

//     // Local boolean to track if ALL plates are pressed this frame
//     private bool allPlatesPressed = false;

//     private void Awake()
//     {
//         // Initialize plate states
//         plateStates = new bool[pressurePlates.Length];
//     }

//     private void FixedUpdate()
//     {
//         bool localAllPressed = true;

//         // 1) Check each plate to see if there's at least one "Player" on it
//         for (int i = 0; i < pressurePlates.Length; i++)
//         {
//             // Use a small 2D box overlap around the plate position
//             Collider2D[] hits = Physics2D.OverlapBoxAll(
//                 pressurePlates[i].transform.position,
//                 new Vector2(0.5f, 0.5f),
//                 0f
//             );

//             bool isPressed = false;
//             foreach (var hit in hits)
//             {
//                 if (hit.CompareTag("Player"))
//                 {
//                     isPressed = true;
//                     break;
//                 }
//             }

//             plateStates[i] = isPressed;
//             if (!isPressed)
//             {
//                 localAllPressed = false;
//             }
//         }

//         // 2) If all plates are pressed now, and previously they weren't, open doors
//         if (localAllPressed && !allPlatesPressed)
//         {
//             allPlatesPressed = true;
//             OpenDoors();
//         }
//         // 3) If not all pressed now, but previously all were pressed, close doors
//         //    (unless we want them to stay open once triggered).
//         else if (!localAllPressed && allPlatesPressed && !stayOpenOnceTriggered)
//         {
//             allPlatesPressed = false;
//             CloseDoors();
//         }
//     }

//     /// <summary>
//     /// Deactivate doors so the path is open.
//     /// </summary>
//     private void OpenDoors()
//     {
//         foreach (var door in doors)
//         {
//             if (door != null)
//             {
//                 door.SetActive(false);
//             }
//         }
//         Debug.Log("Doors opened!");
//     }

//     /// <summary>
//     /// Reactivate doors so the path is closed.
//     /// </summary>
//     private void CloseDoors()
//     {
//         foreach (var door in doors)
//         {
//             if (door != null)
//             {
//                 door.SetActive(true);
//             }
//         }
//         Debug.Log("Doors closed!");
//     }

//     // For visual debugging in the Editor
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.yellow;
//         if (pressurePlates == null) return;

//         foreach (var plate in pressurePlates)
//         {
//             if (plate != null)
//             {
//                 Gizmos.DrawWireCube(plate.transform.position, new Vector3(0.5f, 0.5f, 1f));
//             }
//         }
//     }
// }
