using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float yOffset = 50f;
    [SerializeField] float parallaxFactor = 0.9f; // 1 = same speed as target, 0.5 = half speed
    [SerializeField] float resetDistance = 500f;
    private Vector3 startPosition;

    void Start()
    {
        if (cameraTransform != null)
        {
            startPosition = transform.position - cameraTransform.position;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Match camera's position with parallax effect
        Vector3 targetPosition = cameraTransform.position * parallaxFactor;
        targetPosition.y = cameraTransform.position.y + yOffset;

        // Keep it at the correct height
        //targetPosition.y += yOffset;

        transform.position = targetPosition;

        float distanceFromStart = Vector3.Distance(cameraTransform.position, transform.position);
        if (distanceFromStart > resetDistance)
        {
            startPosition = cameraTransform.position * (1 - parallaxFactor);
        }
    }
}
