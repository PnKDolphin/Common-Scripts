using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The point to orbit around
    public float distance = 5.0f; // Initial distance from the target
    public float xSpeed = 120.0f; // Speed of rotation around the X-axis
    public float ySpeed = 120.0f; // Speed of rotation around the Y-axis
    public float zoomSpeed = 2.0f; // Speed of zooming in/out with the scroll wheel
    public float minDistance = 2.0f; // Minimum distance from the target
    public float maxDistance = 15.0f; // Maximum distance from the target
    public float smoothTime = 0.2f; // Time for smoothing the movement of the target

    private float x = 0.0f;
    private float y = 0.0f;
    private Vector3 targetOffset; // Offset for smoothly changing the target position
    private Vector3 targetPosition; // The new target position
    private Vector3 lastMousePosition; // Store the last mouse position

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        targetPosition = target.position; // Set the initial target position
    }

    void LateUpdate()
    {
        if (target)
        {
            // Rotate the camera when the right mouse button is held down and Shift is NOT pressed
            if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))
            {
                // Apply consistent rotation speed, independent of distance
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // Clamp the vertical rotation to prevent flipping
                y = Mathf.Clamp(y, 15f, 90f); // Clamping X-axis rotation between 15 and 90 degrees
            }

            // Adjust the distance based on the scroll wheel input
            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            // Change the target position when both Shift and right mouse button are pressed
            if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                Vector3 move = new Vector3(-mouseDelta.x * 0.01f, 0, -mouseDelta.y * 0.01f);

                // Move the target only on the X and Z axes
                targetOffset += transform.TransformDirection(move);
                targetOffset.y = 0; // Ensure no change in Y-axis
            }
        }

        lastMousePosition = Input.mousePosition; // Update last mouse position
    }
    
    void Update()
    {
            // Calculate the rotation
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // Apply the rotation and position
            transform.rotation = rotation;
            transform.position = position;

            // Smoothly move the target to the new position, keeping Y position constant
            Vector3 newTargetPosition = targetPosition + targetOffset;
            newTargetPosition.y = target.position.y; // Keep the Y position constant
            target.position = Vector3.Lerp(target.position, newTargetPosition, Time.deltaTime / smoothTime);
    }

    // Call this method to set a new target position (Optional)
    public void SetTargetPosition(Vector3 newPosition)
    {
        targetOffset = new Vector3(newPosition.x - targetPosition.x, 0, newPosition.z - targetPosition.z);
        targetPosition = newPosition;
    }
}
