using UnityEngine;

public class SpriteDirectionChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    private Vector3 lastPosition;
    private Transform camTransform;
    private Billboard billboard;

    [SerializeField] private bool flipRightSide;
    [SerializeField] private bool flipLeftSide;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        camTransform = Camera.main.transform;
        billboard = GetComponentInParent<Billboard>();
    }

    void Update()
    {
        // 1. Calculate movement direction
        Vector3 moveDir = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            // 2. Flatten camera forward vector
            Vector3 camForward = camTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            // 3. Find signed angle relative to camera view
            float angle = Vector3.SignedAngle(camForward, moveDir.normalized, Vector3.up);

            // 4. Choose sprite based on angle quadrants
            if (angle >= -45f && angle <= 45f)
            {
                spriteRenderer.flipX = false;
                spriteRenderer.sprite = backSprite; // Moving away from camera
                billboard.enabled = true;
            }
            else if (angle >= 135f || angle <= -135f)
            {
                spriteRenderer.flipX = false;
                spriteRenderer.sprite = frontSprite; // Moving toward camera
                billboard.enabled = false; // Disable billboard when moving toward camera
            }
            else if (angle > 45f && angle < 135f)
            {
                spriteRenderer.flipX = flipRightSide;
                spriteRenderer.sprite = rightSprite; // Moving right relative to camera
                billboard.enabled = true;
            }
            else
            {
                spriteRenderer.flipX = flipLeftSide ;
                spriteRenderer.sprite = leftSprite;
                billboard.enabled = true;
            }
        }
    }
}
