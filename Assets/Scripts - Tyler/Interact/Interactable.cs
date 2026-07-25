using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float timeSpent = 0f;
    public Renderer interactableHighlight;

    public void Update()
    {
        HighlightInteractable();
    }
    public void Interact()
    {
        if (PlayerManager.Instance.interactable = this)
        {
            Debug.Log($"Interact with: {name}");
            if (TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.material.color = Color.blue;
            }
        }
    }

    private void HighlightInteractable()
    {
        if (PlayerManager.Instance.interactable == this)
            interactableHighlight.enabled = true;
        else
            interactableHighlight.enabled = false;
    }
}