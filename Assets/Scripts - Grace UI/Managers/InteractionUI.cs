using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas: Script for the InteractionPrompt Canvas that pops up when needing to interact with an object
/// </summary>

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private Image keycapImage;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text actionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        HidePrompt();
    }

    public void ShowPrompt(
        Sprite keycapSprite,
        string keyLetter,
        string actionMessage
    )
    {
        if (keycapImage != null)
        {
            keycapImage.sprite = keycapSprite;
        }

        if (keyText != null)
        {
            keyText.text = keyLetter;
        }

        if (actionText != null)
        {
            actionText.text = actionMessage;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}
