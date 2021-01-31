using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockedInteractable : Interactable
{
    [SerializeField] private string lockedText;
    public override void Interact()
    {
        base.Interact();

        if(canInteract)
        {
            PlayerInteractContext.current.ShowText(lockedText);
        }
    }
}
