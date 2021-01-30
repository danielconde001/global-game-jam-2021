using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockedInteractable : Interactable
{
    public override void Interact()
    {
        base.Interact();

        if(canInteract)
        {
            //display somewhere that door is locked
        }
    }
}
