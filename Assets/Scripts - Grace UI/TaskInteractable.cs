using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    [Header("Sound")]
    public bool isRummageable = false;
    public bool isSmoking = false;
    public bool isTalking = false;
    public float stopSoundDelay = 1f;

    private void Update()
    {

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

    public void InteractWithObject()
    {
        if (playerInRange && PlayerManager.Instance.taskInteractable == this)
        {
            if (!taskCompleted)
            {
                if (isRummageable)
                {
                    Debug.Log($"Rummaging through: {name}");
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.genericRummage, stopSoundDelay);
                }

                if (isSmoking)
                {
                    Debug.Log($"Smoking: {name}");
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.smoking);
                }

                if (isTalking)
                {
                    Debug.Log($"Talking to: {name}");
                    AudioManager.Instance.PlaySegment(AudioManager.Instance.goblinNoise, 66f, stopSoundDelay);
                }

                CompleteTask();
            }
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