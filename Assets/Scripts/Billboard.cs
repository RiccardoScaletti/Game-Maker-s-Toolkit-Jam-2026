using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    void Start()
    {
        // Cache the main camera's transform component
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (camTransform == null) return;

        // Calculate target position at the same height as the sprite
        Vector3 targetPosition = camTransform.position;
        targetPosition.y = transform.position.y;

        // Force the sprite to look at the adjusted camera position
        transform.LookAt(targetPosition);
    }
}