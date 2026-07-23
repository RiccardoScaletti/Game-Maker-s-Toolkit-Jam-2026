using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private Sprite keycapSprite;
    [SerializeField] private string promptMessage = "Interact";

    [Header("Interaction")]
    [SerializeField] private UnityEvent onInteract;

    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange)
        {
            return;
        }

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = true;

        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.ShowPrompt(
                keycapSprite,
                promptMessage
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = false;

        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.HidePrompt();
        }
    }

    private void Interact()
    {
        Debug.Log($"Interacted with {gameObject.name}");

        onInteract.Invoke();
    }

    private void OnDisable()
    {
        if (playerInRange && InteractionManager.Instance != null)
        {
            InteractionManager.Instance.HidePrompt();
        }

        playerInRange = false;
    }
}
