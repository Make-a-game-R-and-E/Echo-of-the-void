// using System.Collections.Generic;
// using UnityEngine;

// public class CableDrag : MonoBehaviour
// {
//     private LineRenderer lineRenderer;
//     private Vector3 startPoint;
//     private bool isDragging = false;
//     private Transform targetEnd = null;

//     void Start()
//     {
//         lineRenderer = GetComponent<LineRenderer>();
//         lineRenderer.positionCount = 2;
//         lineRenderer.enabled = false; // הכבל לא נראה בהתחלה
//     }

//     void OnMouseDown()
//     {
//         if (CompareTag("CableStart")) // רק אם זה כבל שמתחילים ממנו
//         {
//             isDragging = true;
//             startPoint = transform.position;
//             lineRenderer.SetPosition(0, startPoint);
//             lineRenderer.enabled = true; // הפעלת הכבל
//         }
//     }

//     void Update()
//     {
//         if (isDragging)
//         {
//             Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             mousePosition.z = 0;
//             lineRenderer.SetPosition(1, mousePosition);
//         }
//     }

//     void OnMouseUp()
//     {
//         if (isDragging)
//         {
//             isDragging = false;

//             if (targetEnd != null) // אם הוא שחרר על תחנה נכונה
//             {
//                 lineRenderer.SetPosition(1, targetEnd.position);
//                 CheckCableConnection(targetEnd.gameObject);
//             }
//             else
//             {
//                 lineRenderer.enabled = false; // אם לא - ביטול הכבל
//             }
//         }
//     }

//     void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("CableEnd"))
//         {
//             targetEnd = collision.transform; // שומר את התחנה הסופית
//         }
//     }

//     void OnTriggerExit2D(Collider2D collision)
//     {
//         if (collision.CompareTag("CableEnd") && collision.transform == targetEnd)
//         {
//             targetEnd = null; // מבטל את התחנה אם השחקן מתרחק ממנה
//         }
//     }

//     void CheckCableConnection(GameObject endStation)
//     {
//         string startName = gameObject.name; // שם התחלת הכבל
//         string endName = endStation.name; // שם התחנה הסופית

//         // בודק אם זה החיבור הנכון לפי ה-CableManager
//         if (CableManager.Instance.IsCorrectConnection(startName, endName))
//         {
//             Debug.Log("Correct Connection! Door opening...");
//         }
//         else
//         {
//             Debug.Log("Incorrect Connection! Try again.");
//             lineRenderer.enabled = false;
//         }
//     }
// }
