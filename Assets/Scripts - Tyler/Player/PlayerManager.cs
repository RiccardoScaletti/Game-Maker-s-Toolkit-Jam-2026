using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private Rigidbody rb;

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

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    Interactable interactable;

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
        InputSystem.actions.FindAction("TurnLeft").performed += OnRotatePlayerLeft;
        InputSystem.actions.FindAction("TurnRight").performed += OnRotatePlayerRight;
        InputSystem.actions.FindAction("Interact").started += OnInteract;
    }

    private void OnDisable()
    {
        InputSystem.actions.Disable();
        InputSystem.actions.FindAction("Move").performed -= OnMovePlayer;
        InputSystem.actions.FindAction("Move").canceled -= context => moveVector = Vector3.zero;
        InputSystem.actions.FindAction("TurnLeft").performed -= OnRotatePlayerLeft;
        InputSystem.actions.FindAction("TurnRight").performed -= OnRotatePlayerRight;
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
        InteractCheck();
        SpriteControl();
    }

    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnRotatePlayerLeft(InputAction.CallbackContext context)
    {
        transform.Rotate(0, -90, 0);
    }

    public void OnRotatePlayerRight(InputAction.CallbackContext context)
    {
        transform.Rotate(0, 90, 0);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (interactable != null)
        {
            //Debug.Log("Interacted with:");
            interactable.interactedWith = true;
            interactable.Interact();
        }
    }

    // Checks for nearby interactables and when interact is pressed it grabs their Interactable component and calls the Interact() method on it
    private void InteractCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactableCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<Interactable>(out interactable))
            {
                break; // Interact with the first interactable found
            }
        }
    }

    private void SpriteControl()
    {
        bool isMoving = rb.linearVelocity.magnitude > 0;
        animator.SetBool("isWalking", isMoving);

        if (moveVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

}