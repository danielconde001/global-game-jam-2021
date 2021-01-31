using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyInteractable : Interactable
{
    [SerializeField] UnityEvent OnInteract;

    public override void Interact()
    {
        base.Interact();
        OnInteract.Invoke();
    }
}
