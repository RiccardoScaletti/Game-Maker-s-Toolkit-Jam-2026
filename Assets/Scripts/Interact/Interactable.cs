using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float timeSpent = 0f;
    public Renderer interactableHighlight;
    public bool isRummageable = false;
    public bool isSmoking = false;
    public bool isTalking = false;

    public float stopSoundDelay = 1f;

    public void Update()
    {
        //HighlightInteractable();
    }
    public void Interact()
    {
        //if (PlayerManager.Instance.interactable = this)
        //{
        //    Debug.Log($"Interact with: {name}");
        //    if (TryGetComponent<Renderer>(out Renderer renderer))
        //    {
        //        renderer.material.color = Color.blue;
        //    }
        //}

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
            AudioManager.Instance.PlaySegment(AudioManager.Instance.goblinNoise, 66f , stopSoundDelay);
        }
    }

    //private void HighlightInteractable()
    //{
    //    if (PlayerManager.Instance.interactable == this)
    //        interactableHighlight.enabled = true;
    //    else
    //        interactableHighlight.enabled = false;
    //}
}