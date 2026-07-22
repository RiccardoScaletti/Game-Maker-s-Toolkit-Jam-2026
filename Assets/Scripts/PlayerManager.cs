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

    [SerializeField] private InputActionAsset Player;
    private InputAction moveForward;
    private InputAction moveLeft;
    private InputAction moveRight;
    private InputAction moveBack;
    private InputAction interact;

    private Vector3 moveValue;

    private CharacterController characterController;

    public bool isPaused = false;
    public bool youLose = false;
    public bool youWin = false;
    public bool inputAllowed = true;

    void awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        Player.Enable();

        moveForward = Player.FindAction("Forward");
        moveLeft = Player.FindAction("Left");
        moveRight = Player.FindAction("Right");
        moveBack = Player.FindAction("Backward");
        interact = Player.FindAction("Interact");

        characterController = GetComponent<CharacterController>();
    }

    void update()
    {
        if (youLose || youWin)
        {
            inputAllowed = false;
        }
        else
        {
            inputAllowed = true;
        }

        moveValue = Vector3.zero;
        if (moveValue != null && !isPaused)
        {
            // Handle movement logic here
        }
        
        if (interact.WasPressedThisFrame() && !isPaused)
        {
            // Handle interaction logic here
        }
    }
}
