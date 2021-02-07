using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyInteractable : Interactable
{
    public override void Interact()
    {
        base.Interact();
        OnInteract.Invoke();
    }
}
