using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChestInteractable : Interactable
{
    public override void Interact()
    {
        if(canInteract)
        {
            RoomSpawner.current.PlayRickRoll();
            Application.Quit();
        }
    }
}
