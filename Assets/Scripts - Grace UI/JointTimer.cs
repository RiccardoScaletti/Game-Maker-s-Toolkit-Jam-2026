using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Timer: Controls the joint timer, visuals, and multiplyers for different level tasks - hard, medium, easy
/// </summary>

public class JointTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField, Min(1f)]
    private float startingDuration = 600f;

    [SerializeField, Min(0f)]
    private float burnSpeedMultiplier = 1f;

    [SerializeField]
    private bool beginAutomatically = true;

    [SerializeField]
    private bool continueWhileGamePaused = false;

    [Header("Joint Visual")]
    [Tooltip("The part of the joint that visually shrinks.")]
    [SerializeField]
    private RectTransform jointBody;

    [Tooltip("The glowing end that moves as the joint burns.")]
    [SerializeField]
    private RectTransform ember;

    [Tooltip("Ember position when the joint is completely unburned.")]
    [SerializeField]
    private Transform emberStartPoint;

    [Tooltip("Ember position when the joint is completely burned.")]
    [SerializeField]
    private Transform emberEndPoint;

    [Header("Optional UI")]
    [SerializeField]
    private TMP_Text timerText;

    [Header("Ember Glow")]
    [SerializeField] private Image emberImage;
    [SerializeField] private Gradient emberGradient;

    [Header("Testing")]
    [SerializeField] private bool enableTestKey = true;

    [SerializeField]
    private GameObject smokeEffect;

    private float remainingTime;
    private float originalJointWidth;
    private bool isRunning;
    private bool hasFinished;

    public float RemainingTime => remainingTime;
    public float BurnSpeedMultiplier => burnSpeedMultiplier;
    public bool IsRunning => isRunning;

    public event Action TimerFinished;

    [Header("Burn Multiplier")]
    [SerializeField] private float burnMultiplier = 1f;
    [SerializeField] private float multiplierIncrease = 0.25f;
    [SerializeField] private float maxMultiplier = 3f;



    private void Awake()
    {
        remainingTime = startingDuration;

        if (jointBody != null)
        {
            originalJointWidth = jointBody.sizeDelta.x;
        }

        UpdateVisuals();
    }

    private void Start()
    {
        if (beginAutomatically)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (!isRunning || hasFinished)
        {
            return;
        }

        float frameTime = continueWhileGamePaused
            ? Time.unscaledDeltaTime
            : Time.deltaTime;

        remainingTime -= frameTime * burnSpeedMultiplier;
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateVisuals();

        if (remainingTime <= 0f)
        {
            FinishTimer();
        }

        remainingTime -= Time.deltaTime * burnMultiplier;

        if (enableTestKey &&
    Keyboard.current != null &&
    Keyboard.current.tKey.wasPressedThisFrame)
        {
            TaskCompleted();
        }

        remainingTime -= Time.deltaTime * burnMultiplier;
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateVisuals();

        if (remainingTime <= 0f)
        {
            FinishTimer();
        }
    }

    public void StartTimer()
    {
        if (hasFinished)
        {
            return;
        }

        isRunning = true;

        if (smokeEffect != null)
        {
            smokeEffect.SetActive(true);
        }
    }

    public void PauseTimer()
    {
        isRunning = false;

        if (smokeEffect != null)
        {
            smokeEffect.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        remainingTime = startingDuration;
        burnMultiplier = 1f;
        hasFinished = false;
        isRunning = false;

        UpdateVisuals();
        UpdateEmberColor();
    }

    public void SetBurnMultiplier(float newMultiplier)
    {
        burnSpeedMultiplier = Mathf.Max(0f, newMultiplier);
    }

    public void MultiplyBurnSpeed(float multiplierAmount)
    {
        if (multiplierAmount <= 0f)
        {
            Debug.LogWarning("Burn multiplier must be greater than zero.");
            return;
        }

        burnSpeedMultiplier *= multiplierAmount;
    }

    private void UpdateVisuals()
    {
        float remainingPercent = remainingTime / startingDuration;
        remainingPercent = Mathf.Clamp01(remainingPercent);

        UpdateJointLength(remainingPercent);
        UpdateEmberPosition(remainingPercent);
        UpdateTimerText();
    }

    private void UpdateJointLength(float remainingPercent)
    {
        if (jointBody == null)
        {
            return;
        }

        Vector2 newSize = jointBody.sizeDelta;
        newSize.x = originalJointWidth * remainingPercent;
        jointBody.sizeDelta = newSize;
    }

    private void UpdateEmberPosition(float remainingPercent)
    {
        if (ember == null || emberStartPoint == null || emberEndPoint == null)
        {
            return;
        }

        float burnedPercent = 1f - remainingPercent;

        ember.position = Vector3.Lerp(
            emberStartPoint.position,
            emberEndPoint.position,
            burnedPercent
        );
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }

        int totalSeconds = Mathf.CeilToInt(remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void FinishTimer()
    {
        remainingTime = 0f;
        isRunning = false;
        hasFinished = true;

        if (smokeEffect != null)
        {
            smokeEffect.SetActive(false);
        }

        UpdateVisuals();

        TimerFinished?.Invoke();

        Debug.Log("Joint timer finished.");
    }

    public enum TaskType
    {
        Easy,
        Medium,
        Hard
    }

    public void TaskCompleted()
    {
        burnMultiplier += multiplierIncrease;
        burnMultiplier = Mathf.Clamp(
            burnMultiplier,
            1f,
            maxMultiplier
        );

        UpdateEmberColor();

        Debug.Log(
            $"Task completed! Burn multiplier: {burnMultiplier:0.00}x"
        );
    }

    private void UpdateEmberColor()
    {
        if (emberImage == null)
            return;

        float t = Mathf.InverseLerp(1f, maxMultiplier, burnMultiplier);

        emberImage.color = emberGradient.Evaluate(t);
    }
}
