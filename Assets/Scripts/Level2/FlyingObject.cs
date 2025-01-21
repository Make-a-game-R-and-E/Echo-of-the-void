using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] float speed = 10f; // מהירות התנועה

    void FixedUpdateNetwork()
    {
        // move the object left
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
