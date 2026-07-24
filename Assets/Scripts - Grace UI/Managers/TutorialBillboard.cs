using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialBillboard : MonoBehaviour
{
    [Header("Billboard UI")]
    [SerializeField] private GameObject billboardCanvas;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image tutorialImage;

    [Header("Tutorial Content")]
    [SerializeField] private string tutorialTitle = "Tutorial";

    [TextArea(3, 8)]
    [SerializeField]
    private string tutorialDescription =
        "Enter your tutorial instructions here.";

    [SerializeField] private Sprite tutorialSprite;

    [Header("Behavior")]
    [SerializeField] private bool hideWhenPlayerLeaves = true;
    [SerializeField] private bool showOnlyOnce = false;
    [SerializeField] private bool faceCamera = true;

    private bool hasBeenShown;
    private Transform cameraTransform;

    private void Awake()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        ApplyTutorialContent();

        if (billboardCanvas != null)
        {
            billboardCanvas.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!faceCamera ||
            billboardCanvas == null ||
            !billboardCanvas.activeSelf ||
            cameraTransform == null)
        {
            return;
        }

        Vector3 direction =
            billboardCanvas.transform.position -
            cameraTransform.position;

        direction.y = -3f;

        if (direction.sqrMagnitude > 0.001f)
        {
            billboardCanvas.transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (showOnlyOnce && hasBeenShown)
        {
            return;
        }

        ShowBillboard();
        hasBeenShown = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (hideWhenPlayerLeaves)
        {
            HideBillboard();
        }
    }

    public void ShowBillboard()
    {
        ApplyTutorialContent();

        if (billboardCanvas != null)
        {
            billboardCanvas.SetActive(true);
        }
    }

    public void HideBillboard()
    {
        if (billboardCanvas != null)
        {
            billboardCanvas.SetActive(false);
        }
    }

    private void ApplyTutorialContent()
    {
        if (titleText != null)
        {
            titleText.text = tutorialTitle;
        }

        if (descriptionText != null)
        {
            descriptionText.text = tutorialDescription;
        }

        if (tutorialImage != null)
        {
            tutorialImage.sprite = tutorialSprite;
            tutorialImage.gameObject.SetActive(
                tutorialSprite != null
            );
        }
    }
}
