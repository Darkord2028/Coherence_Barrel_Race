using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;                  // Player or car
    [SerializeField] Vector3 offset = new Vector3(0f, 5f, -10f); // Relative to target

    [Header("Follow Settings")]
    [SerializeField] float positionSmoothTime = 0.1f;
    [SerializeField] float rotationSmoothTime = 0.1f;
    [SerializeField] bool followRotation = true;
    [SerializeField] bool lookAtTarget = true;

    [Header("Clamp Settings")]
    [SerializeField] float minY = 2f;
    [SerializeField] float maxY = 20f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 currentVelocity;
    private Quaternion currentRotationVelocity;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Desired camera position relative to target
        Vector3 desiredPosition = target.TransformPoint(offset);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // Smoothly interpolate to desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionSmoothTime);

        if (followRotation)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
        }

        if (lookAtTarget && !followRotation)
        {
            transform.LookAt(target.position + Vector3.up * 1.5f); // look at chest/head height
        }

        // Optional: Handle camera collision here (to avoid clipping through walls)
        // HandleCameraCollision();
    }

    // Optional stub if you want to handle camera-wall collision
    /*
    void HandleCameraCollision()
    {
        RaycastHit hit;
        Vector3 dir = (transform.position - target.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        if (Physics.Raycast(target.position, dir, out hit, distance))
        {
            transform.position = hit.point - dir * 0.5f; // offset a bit from the wall
        }
    }
    */

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

}
