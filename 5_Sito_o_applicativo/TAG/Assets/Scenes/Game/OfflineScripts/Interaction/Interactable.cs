using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Add or remove an interactionEvent component to this GameObject
    public bool useEvents;
    // Message displayed to player when looking at an interactable
    [SerializeField]
    public string promptMessage;

    // This function will be called from our player
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {
        // We won't have any code written in this function
        // This is a template function to be overridden by our subclasses
    }
}
