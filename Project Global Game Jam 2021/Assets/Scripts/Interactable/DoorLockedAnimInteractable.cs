using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockedAnimInteractable : DoorLockedInteractable
{
    [SerializeField] protected Animator selfAnimator;
    [SerializeField] protected AudioClip clipDoorOpen;

    public void OpenDoor()
    {
        selfAnimator.SetTrigger("Open Door");
        selfAudioSource.PlayOneShot(clipDoorOpen);
        canInteract = false;
    }
}
