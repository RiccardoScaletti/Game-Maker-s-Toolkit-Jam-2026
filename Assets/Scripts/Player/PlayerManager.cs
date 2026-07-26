using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Rigidbody rb;

    public float interactableCheckRadius = 3f;

    private Vector2 moveVector;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxForce;

    private bool youLose;
    public bool YouLose
    {
        get { return youLose; }
        set
        {
            youLose = value;
            if (youLose)
                InputSystem.actions.Disable();
        }
    }
    private bool youWin;
    public bool YouWin 
    {
        get { return youWin; }
        set
        {
            youWin = value;
            if (youWin)
                InputSystem.actions.Disable();
        }
    }

    private bool isCaptured;
    public bool IsCaptured
    {
        get { return isCaptured; }
        set
        {
            isCaptured = value;
            if (isCaptured)
            {
                //interactable = null;
                taskInteractable = null;
                InputSystem.actions.Disable();
            }
            else
                InputSystem.actions.Enable();
        }
    }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    //public Interactable interactable;
    public TaskInteractable taskInteractable;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Excess instance of player singleton");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputSystem.actions.Enable();
        InputSystem.actions.FindAction("Move").performed += OnMovePlayer;
        InputSystem.actions.FindAction("Move").canceled += context => moveVector = Vector3.zero;
        InputSystem.actions.FindAction("Interact").started += OnInteract;
    }

    private void OnDisable()
    {
        InputSystem.actions.Disable();
        InputSystem.actions.FindAction("Move").performed -= OnMovePlayer;
        InputSystem.actions.FindAction("Move").canceled -= context => moveVector = Vector3.zero;
        InputSystem.actions.FindAction("Interact").started -= OnInteract;
    }

    private void FixedUpdate()
    {
        // Find target velocity
        Vector3 curVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveVector.x, 0 ,moveVector.y);

        // align direction
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= moveSpeed;

        // Calculate forces
        Vector3 velocityChange = targetVelocity - curVelocity;
        velocityChange.y = 0;   // remove y calculation so that gravity isn't overriden

        // limit force
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Update()
    {
        //InteractCheck();
        SpriteControl();
    }

    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        if (isCaptured)
        {
            return;
        }
        moveVector = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<TaskInteractable>(out taskInteractable);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //if (interactable != null)
        //    interactable.Interact();
        if (isCaptured)
        {
            return;
        }
        if (taskInteractable != null)
            taskInteractable.InteractWithObject();
    }

    // Checks for nearby interactables and when interact is pressed it grabs their Interactable component and calls the Interact() method on it
    //private void InteractCheck()
    //{
    //    if (isCaptured)
    //        return;
    //
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactableCheckRadius);
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.TryGetComponent<Interactable>(out interactable))
    //        {
    //            break; // Interact with the first interactable found
    //        }
    //    }
    //}

    private void SpriteControl()
    {
        bool isLeft = moveVector.x < 0;
        bool isRight = moveVector.x > 0;
        bool isForward = moveVector.y > 0;
        bool isBackward = moveVector.y < 0;

        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isRight", isRight);
        animator.SetBool("isForward", isForward);
        animator.SetBool("isBackward", isBackward);
        animator.SetBool("isWalking", moveVector.sqrMagnitude > 0.01f);

        if (moveVector.x > 0)
            spriteRenderer.flipX = false;
        else if (moveVector.x < 0)
            spriteRenderer.flipX = true;
    }

    public void CapturePlayer(GameObject capturePoint)
    {
        IsCaptured = true;
        rb.Sleep();

        rb.detectCollisions = false;
        rb.useGravity = false;

        transform.SetParent(capturePoint.transform);
        transform.SetPositionAndRotation(capturePoint.transform.position, Quaternion.identity);
    }

    public void ReleasePlayerFromCapture()
    {
        IsCaptured = false;
        rb.WakeUp();

        rb.detectCollisions = true;
        rb.useGravity = true;

        transform.parent = null;
    }

    public void LockPlayer()
    {
        moveVector = Vector2.zero;   // stop immediately, don't wait for the canceled event
        InputSystem.actions.Disable();
    }

    public void UnlockPlayer()
    {
        // Don't re-enable input if something else locked it while the task was running
        if (!youLose && !youWin && !isCaptured)
            InputSystem.actions.Enable();
    }

    public void PlayTaskAnimation(TaskInteractable.TaskAnimation anim)
    {
        switch (anim)
        {
            case TaskInteractable.TaskAnimation.Smoking:
                animator.SetTrigger("smokingTrigger");
                break;
            case TaskInteractable.TaskAnimation.Goose:
                animator.SetTrigger("goose");
                break;
            case TaskInteractable.TaskAnimation.Orb:
                animator.SetTrigger("orb");
                break;
        }
    }
}