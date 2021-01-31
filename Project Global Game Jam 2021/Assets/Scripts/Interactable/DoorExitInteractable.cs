using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorExitInteractable : Interactable
{
    [SerializeField] protected Room room;

    public UnityEvent OnInteractLocked;
    public UnityEvent OnInteractUnlocked;

    public bool unlocked = true;

    public override void Interact()
    {
        base.Interact();
        
        if (unlocked) room.HasFinishedRoom();
        if (!unlocked) OnInteractLocked.Invoke();

    }

    public void ShowText(string interactText)
    {
        PlayerInteractContext.current.ShowText(interactText);
    }

    public void Unlock()
    {
        unlocked = true;
    }
}
