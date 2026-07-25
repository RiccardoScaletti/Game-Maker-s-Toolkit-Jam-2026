using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private PlayerInput playerInputActions;
    private CharacterController controller;

    public float interactableCheckRadius = 3f;

    private Vector3 moveDirection;
    private Vector2 inputVector;

    [SerializeField] private float moveSpeed = 5f;

    public bool isPaused = false;
    public bool youLose = false;
    public bool youWin = false;
    public bool inputAllowed = true;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Excess instance of player singleton");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controller = GetComponent<CharacterController>();
        playerInputActions = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerInputActions.enabled = true;
    }

    private void OnDisable()
    {
        playerInputActions.enabled = false;
    }

    void Update()
    {
        if (youLose || youWin)
        {
            inputAllowed = false;
        }
        else
        {
            inputAllowed = true;
        }

        Movement();
        InteractCheck();
        RotateCharacter();
        SpriteControl();
    }

    // Moves the player relative to the player object's forward and right directions based on the input vector from the PlayerInput component
    private void Movement()
    {
        if (inputAllowed)
        {
            inputVector = playerInputActions.actions["Move"].ReadValue<Vector2>();
            moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
            moveDirection = transform.TransformDirection(moveDirection);
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    // Checks for nearby interactables and when interact is pressed it grabs their Interactable component and calls the Interact() method on it
    private void InteractCheck()
    {
        if (playerInputActions.actions["Interact"].WasPressedThisFrame() && !isPaused)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactableCheckRadius);
            foreach (var hitCollider in hitColliders)
            {
                Interactable interactable = hitCollider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.interactedWith = true;
                    interactable.Interact();
                    break; // Interact with the first interactable found
                }
            }
        }
    }

    // Rotate 90 degree the cinemachine handles the rotation of the camera so we just need to rotate the player object
    private void RotateCharacter()
    {
        if (playerInputActions.actions["TurnLeft"].WasPressedThisFrame() && !isPaused)
            transform.Rotate(0f, -90f, 0f);
        if (playerInputActions.actions["TurnRight"].WasPressedThisFrame() && !isPaused)
            transform.Rotate(0f, 90f, 0f);
    }

    private void SpriteControl()
    {
        bool isMoving = inputVector.magnitude > 0;
        animator.SetBool("isWalking", isMoving);

        if (inputVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}