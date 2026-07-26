using UnityEngine;

public class SpriteDirectionChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;
    private Transform camTransform;

    private Animator animator;

    [SerializeField] private bool flipRightSide;
    [SerializeField] private bool flipLeftSide;

    public bool isGranny;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        camTransform = Camera.main.transform;
    }
    private void OnEnable()
    {
        if (isGranny)
            enabled = false;
    }
    void Update()
    {
        SpriteSwap();
    }

    private void SpriteSwap()
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
            // Moving Backward
            if (angle >= -45f && angle <= 45f)
            {
                animator.SetBool("isBackward", true);
                animator.SetBool("isForward", false);
                animator.SetBool("isSideways", false);
                spriteRenderer.flipX = false;
            }
            // Moving Forward
            else if (angle >= 135f || angle <= -135f)
            {
                animator.SetBool("isBackward", false);
                animator.SetBool("isForward", true);
                animator.SetBool("isSideways", false); 
                spriteRenderer.flipX = false;
            }
            // Moving Right
            else if (angle > 45f && angle < 135f)
            {
                animator.SetBool("isBackward", false);
                animator.SetBool("isForward", false);
                if (!isGranny)
                    animator.SetBool("isSideways", true);
                else if (isGranny)
                {
                    animator.SetBool("isSideLeft", false);
                    animator.SetBool("isSideRight", true);
                }
                spriteRenderer.flipX = flipRightSide;
            }
            // Moving Left
            else
            {
                animator.SetBool("isBackward", false);
                animator.SetBool("isForward", false);
                if (!isGranny)
                    animator.SetBool("isSideways", true);
                else if (isGranny)
                {
                    animator.SetBool("isSideLeft", true);
                    animator.SetBool("isSideRight", false);
                }
                spriteRenderer.flipX = flipLeftSide;
            }
        }
    }
}