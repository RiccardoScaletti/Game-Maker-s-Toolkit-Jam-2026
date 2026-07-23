using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject player;
    public float distance = 3f;
    public float timeSpent = 0f;
    public bool canInteract = false;
    public bool interactedWith = false;
    public Renderer interactableHighlight;

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
        canInteract = distanceToPlayer <= distance;
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
                }
            }
        }
    }

    private void HighlightInteractable()
    {
        if (canInteract)
        {
            interactableHighlight.enabled = true;
        }
        else
        {
            interactableHighlight.enabled = false;
        }
    }
}
