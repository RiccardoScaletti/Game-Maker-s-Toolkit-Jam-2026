using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance
    {
        get
        {
            return Instance;
        }
    }

    private static PlayerManager instance = null;

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

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        controller = GetComponent<CharacterController>();
        playerInputActions = GetComponent<PlayerInput>();
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

    // Rotate 90 degree with a lerp so it looks smoother using input TurnLeft and TurnRight from the PlayerInput component. The rotation should be relative to the player object's forward direction.
    private void RotateCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            transform.Rotate(0f, -90f, 0f);
        if (Input.GetKeyDown(KeyCode.E))
            transform.Rotate(0f, 90f, 0f);
    }
}
