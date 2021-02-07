using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChestInteractable : Interactable
{
    [SerializeField] protected AudioSource selfAudioSource;
    [SerializeField] protected AudioClip clipChestOpen;

    public override void Interact()
    {
        if(canInteract)
        {
            GameManager.Instance.PauseMenuManager.CanPause = false;
            RoomSpawner.current.PlayRickRoll();
            selfAudioSource.clip = clipChestOpen;
            selfAudioSource.Play();
        }
    }
}
