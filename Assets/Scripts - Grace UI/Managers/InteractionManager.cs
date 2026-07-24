using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private Image keycapImage;
    [SerializeField] private TMP_Text interactionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("More than one InteractionUI exists.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        HidePrompt();
    }

    public void ShowPrompt(
        Sprite keycapSprite,
        string promptMessage
    )
    {
        if (keycapImage != null)
        {
            keycapImage.sprite = keycapSprite;
            keycapImage.enabled = keycapSprite != null;
        }

        if (interactionText != null)
        {
            interactionText.text = promptMessage;
        }

        if (promptPanel != null)
        {
            promptPanel.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }
}
