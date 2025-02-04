using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ObjectMatchingGame : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private int matchId;
    private bool isDragging;
    private Vector3 endPoint;
    private ObjectMatchForm objectMatchForm;

    // שני משתנים לשני שחקנים
    [SerializeField] private Camera player1Camera;  // מצלמת שחקן 1
    [SerializeField] private Camera player2Camera;  // מצלמת שחקן 2

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        // בחר את המצלמה המתאימה (לפי שחקן פעיל)
        Camera currentCamera = player1Camera; // כאן אתה יכול לשנות את המצלמה לפי השחקן הפעיל
        if (Input.GetKey(KeyCode.Alpha2))  // למשל, לחיצה על מקש 2 לשחקן 2
        {
            currentCamera = player2Camera;
        }

        // אם המצלמה קיימת, בצע את הקריאה ל-Raycast
        if (currentCamera != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(currentCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    Vector3 mousePosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0f;
                    lineRenderer.SetPosition(0, mousePosition);
                }
            }

            if (isDragging)
            {
                Vector3 mousePosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                lineRenderer.SetPosition(1, mousePosition);
                endPoint = mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
                if (hit.collider != null && hit.collider.TryGetComponent(out objectMatchForm) && matchId == objectMatchForm.Get_ID())
                {
                    Debug.Log("Correct connection!");
                    this.enabled = false;
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        isDragging = false;
                        RaycastHit2D hit1 = Physics2D.Raycast(endPoint, Vector2.zero);

                        if (hit1.collider != null && hit1.collider.TryGetComponent(out objectMatchForm) && matchId == objectMatchForm.Get_ID())
                        {
                            Debug.Log("Correct connection!");
                            this.enabled = false;  // מכבה את הסקריפט כך שלא ימשיכו לגרור
                        }
                        else
                        {
                            ResetLine();  // במקום לאפס לגמרי את המונה, נקרא לפונקציה שתעשה זאת מסודר
                        }
                    }

                }

                lineRenderer.positionCount = 2;
            }
        }
        else
        {
            Debug.LogError("Camera not assigned for the current player!");
        }
    }

    private void ResetLine()
{
    lineRenderer.positionCount = 2;
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, transform.position);
}

}

