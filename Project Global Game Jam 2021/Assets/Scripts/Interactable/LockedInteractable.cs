﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockedInteractable : Interactable
{
    [SerializeField] UnityEvent OnInteractLocked;
    [SerializeField] UnityEvent OnInteractUnlocked;

    protected bool locked = true;

    public override void Interact()
    {
        base.Interact();
        if (locked) OnInteractLocked.Invoke();
        if (!locked) OnInteractUnlocked.Invoke();
    }

    public void CanUnlock()
    {
        locked = false;
    }

    public void ShowText(string interactText)
    {
        PlayerInteractContext.current.ShowText(interactText);
    }

    public void ObtainKeyPiece()
    {
        PuzzleManager.Instance.PieceAttained();
        canInteract = false;
    }
}
