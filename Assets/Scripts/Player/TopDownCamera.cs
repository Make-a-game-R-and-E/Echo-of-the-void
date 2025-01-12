using System;
using UnityEngine;

/**
 * Modified from Fusion tutorial for a Top-Down 2D Camera
 */
public class TopDownCamera : MonoBehaviour
{

    [Header("Camera Offset")]
    [SerializeField] Vector3 offset = new Vector3(0, 0, -11); // Default offset above the target

    private Transform target;


    /// <summary>
    /// Assigns the target for the camera to follow.
    /// </summary>
    /// <param name="transform">The target's transform.</param>
    public void SetTarget(Transform transform)
    {
        this.target = transform;
    }

    void Start()
    {
        // Ensure the camera is orthographic for 2D
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.orthographic = true;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position based on the target's position and the offset
        Vector3 desiredPosition = target.position + offset;

        // Update the camera's position and rotation
        transform.position = desiredPosition;
    }
}
