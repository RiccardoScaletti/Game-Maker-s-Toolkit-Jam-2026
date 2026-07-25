using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject player;
    public float distance = 3f;
    public float timeSpent = 0f;
    private bool canInteract = false;
    public bool interactedWith = false;
    public Renderer interactableHighlight;
    private bool blockInteraction = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        CheckPlayerDistance();
        Interact();
        HighlightInteractable();
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!blockInteraction && distanceToPlayer <= distance)
            canInteract = distanceToPlayer <= distance;
        else if (distanceToPlayer >= distance)
            canInteract = false;
    }

    public void Interact()
    {
        if (canInteract)
        {
            if (interactedWith)
            {
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.blue;
                    interactedWith = false;
                    blockInteraction = true;
                }
            }
        }
    }

    private void HighlightInteractable()
    {
        if (canInteract)
            interactableHighlight.enabled = true;
        else if (!canInteract)
            interactableHighlight.enabled = false;
    }
}