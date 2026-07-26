using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum TaskType { Easy, Medium, Hard }

/// <summary>
/// Timer: Controls the joint timer, visuals, and multiplyers for different level tasks - hard, medium, easy
/// </summary>

public class JointTimer : MonoBehaviour
{
    public event Action TimerFinished;

    [Header("Timer")]
    [SerializeField, Min(1f)]
    private float startingDuration = 400f;

    [SerializeField]
    [Tooltip("How fast the multiplier decreases over time. Note: negative numbers will increase burn speed")]
    private float multiplierDecreaseSpeed = 0;

    [SerializeField]
    private float maxMultiplier = 10;

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

    [SerializeField]
    private GameObject smokeEffect;

    private float originalJointWidth;
    private bool isRunning;
    private bool hasFinished;

    public float RemainingTime { get; private set; }

    private float burnSpeedMultiplier;
    public float BurnSpeedMultiplier { 
        get { return burnSpeedMultiplier; } 
        set {
            burnSpeedMultiplier = Mathf.Clamp(value, 0, maxMultiplier); 
        }
    }
    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            isRunning = value;

            UpdateVisuals();
            UpdateEmberColor();

            if (smokeEffect)
                smokeEffect.SetActive(isRunning);
            if (!isRunning)
                enabled = false;
        }
    }

    public bool HasFinished
    {
        get { return hasFinished; }
        set
        {
            hasFinished = value;

            UpdateVisuals();
            UpdateEmberColor();

            if (hasFinished)
                enabled = false;
        }
    }

    private void Awake()
    {
        RemainingTime = startingDuration;

        if (jointBody != null)
            originalJointWidth = jointBody.sizeDelta.x;

        BurnSpeedMultiplier = 0;

        UpdateVisuals();
    }

    private void Start()
    {
        if (beginAutomatically)
            StartTimer();
    }

    private void Update()
    {
        float frameTime = continueWhileGamePaused ? Time.unscaledDeltaTime : Time.deltaTime;

        RemainingTime -= frameTime * BurnSpeedMultiplier;
        BurnSpeedMultiplier -= frameTime * multiplierDecreaseSpeed;

        UpdateVisuals();

        if (RemainingTime <= 0f)
        {
            FinishTimer();
        }
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void ResetTimer()
    {
        RemainingTime = startingDuration;
        BurnSpeedMultiplier = 0;
        //emberValue = 1f;
        HasFinished = false;
        IsRunning = false;
    }

    public void SetBurnMultiplier(float newMultiplier)
    {
        BurnSpeedMultiplier = newMultiplier;
    }
    private void UpdateVisuals()
    {
        float remainingPercent = RemainingTime / startingDuration;
        remainingPercent = Mathf.Clamp01(remainingPercent);

        UpdateJointLength(remainingPercent);
        UpdateEmberPosition(remainingPercent);
        UpdateEmberColor();
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

        int totalSeconds = Mathf.CeilToInt(RemainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void FinishTimer()
    {
        RemainingTime = 0f;
        IsRunning = false;
        HasFinished = true;

        TimerFinished?.Invoke();
        SceneManager.LoadScene("WinScene");

        Debug.Log("Joint timer finished.");
    }

    private void UpdateEmberColor()
    {
        if (emberImage == null)
            return;

        float t = Mathf.InverseLerp(0f, maxMultiplier, BurnSpeedMultiplier);

        emberImage.color = emberGradient.Evaluate(t);
    }
}
