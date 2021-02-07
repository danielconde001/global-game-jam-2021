using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockedInteractable : Interactable
{
    [SerializeField] protected AudioSource selfAudioSource;
    [SerializeField] protected AudioClip audioLock;
    [SerializeField] protected string lockedText;
    public override void Interact()
    {
        if(canInteract)
        {
            if(interactDelay > 0.0f)
            {
                PlayerInteractContext.current.ShowText(lockedText);
                selfAudioSource.Play();
                StartCoroutine(InteractDelayTimer());
            }
        }
    }

    protected void Start()
    {
        selfAudioSource.clip = audioLock;
    }
}
