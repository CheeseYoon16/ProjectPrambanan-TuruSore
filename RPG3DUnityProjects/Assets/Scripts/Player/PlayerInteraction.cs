using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] InteractPromptUI interactPromptUI = null;

    private InteractableObject detectedObject;

    private InteractableObject currentInteractable
    {
        get
        {
            return detectedObject;
        }
        set
        {
            if(value!=null)
            {
                Debug.Log($"Detected interactible object {value.gameObject.name}");
            }

            if(detectedObject != null)
            {
                if (detectedObject != value)
                {
                    detectedObject.SetToInteractable(false);
                }
            }

            detectedObject = value;

            if (detectedObject != null)
            {
                detectedObject.SetToInteractable(true);
            }
        }
    }

    bool disabledInteraction= false;

    void DetectObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, detectionRadius);
        InteractableObject foundInteractable = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out InteractableObject targetObject))
            {
                foundInteractable = targetObject;
                break;
            }
        }

        // Show or hide prompt based on presence
        InteractionPromptData currentPrompt = null;
        Vector3 position = Vector3.zero;
        if(foundInteractable != null)
        {
            currentPrompt = foundInteractable.PromptData;
            position = foundInteractable.transform.position;
        }

        ShowPrompt(foundInteractable != null, position, currentPrompt);

        // Assign detected interactable (this will automatically handle outline toggling)
        currentInteractable = foundInteractable;

    }

    private void Interact()
    {
        if(currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void ShowPrompt(bool show, Vector3 pos , InteractionPromptData promptData = null)
    {
        if(!disabledInteraction)
        {
            if (interactPromptUI != null)
            {
                interactPromptUI.SetPromptEnabled(show, pos, promptData);
            }
        }

    }

    private void Update()
    {
        if(!disabledInteraction)
        {
            DetectObject();

            if (currentInteractable != null && Input.GetKeyDown(KeyCode.E)) //If there is object to interact, then i can interact
            {
                SoundManager.Instance.PlaySound(SoundType.INTERACTION, "Interact");
                Interact();
            }
        }

    }

    public void SetInteractionDisabled(bool active)
    {
        disabledInteraction = active;
    }

}
