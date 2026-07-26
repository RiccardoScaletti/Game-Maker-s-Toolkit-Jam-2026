using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Interaction: The script that is put onto whatever object you want the InteractionPromnpt Canvas to show up
/// </summary>

[RequireComponent(typeof(Collider))]
public class TaskInteractable : MonoBehaviour
{

    [Header("Prompt")]
    [SerializeField] private Sprite keycapSprite;
    [SerializeField] private string keyLetter = "E";
    [SerializeField] private string actionMessage = "Complete Task";

    [Header("Task")]
    [SerializeField] private bool canOnlyCompleteOnce = true;
    [SerializeField] private UnityEvent onTaskCompleted;

    private bool playerInRange;
    private bool taskCompleted;

    /*private void Start()
    {
        CigCountManager.CigCount += 0;
    }*/

    private void Update()
    {
        if (!playerInRange || taskCompleted)
        {
            return;
        }

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            CompleteTask();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = true;

        if (!taskCompleted && InteractionUI.Instance != null)
        {
            InteractionUI.Instance.ShowPrompt(
                keycapSprite,
                keyLetter,
                actionMessage
            );
        }

        //Debug Tester Code: Can get rid of once it has successfully triggered the player nearby
        
        Debug.Log($"Something entered the trigger: {other.name}");

        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = true;

        if (!taskCompleted && InteractionUI.Instance != null)
        {
            InteractionUI.Instance.ShowPrompt(
                keycapSprite,
                keyLetter,
                actionMessage
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

        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.HidePrompt();
        }
    }

    private void CompleteTask()
    {
        Debug.Log($"Task completed: {gameObject.name}");

        onTaskCompleted.Invoke();
        CigCountManager.CigCount += 1;

        if (canOnlyCompleteOnce)
        {
            taskCompleted = true;
        }

        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.HidePrompt();
        }
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (playerInRange && InteractionUI.Instance != null)
        {
            InteractionUI.Instance.HidePrompt();
        }

        playerInRange = false;
    }

    
}
