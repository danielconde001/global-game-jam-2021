﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected Transform _UIContextPosition;
    public Transform UIContextPosition
    {
        get {return _UIContextPosition;}
    }
    
    [SerializeField] protected bool canInteract = true;
    public bool CanInteract
    {
        get {return canInteract;}
        set {canInteract = value;}
    }

    [SerializeField] protected float interactDelay = 0.0f;

    public virtual void Interact()
    {
        if(canInteract)
        {
            if(interactDelay > 0.0f)
            {
                StartCoroutine(InteractDelayTimer());
            }
        }
    }

    public virtual void Select()
    {

    }

    public virtual void Unselect()
    {
        
    }

    protected IEnumerator InteractDelayTimer()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactDelay);
        canInteract = true;
    }
}
