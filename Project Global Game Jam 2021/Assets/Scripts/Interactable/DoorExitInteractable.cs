using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExitInteractable : Interactable
{
    [SerializeField] protected Room room;

    public override void Interact()
    {
        base.Interact();

        room.HasFinishedRoom();
    }
}
