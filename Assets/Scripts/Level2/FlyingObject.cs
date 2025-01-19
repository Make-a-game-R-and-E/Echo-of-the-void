using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    public float speed = 5f; // מהירות התנועה

    void Update()
    {
        // תנועה בכיוון מוגדר (למשל שמאלה)
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
