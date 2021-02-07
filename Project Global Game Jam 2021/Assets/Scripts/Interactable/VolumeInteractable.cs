using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeInteractable : Interactable
{
    [SerializeField] protected Collider selfCollider;
    [SerializeField] protected LayerMask playerLayer;
    [TextArea(2, 3)]
    [SerializeField] protected string playerText;
    [SerializeField] protected bool interactOnce = true;

    public override void Interact()
    {
        base.Interact();

        if(canInteract)
        {
            PlayerInteractContext.current.ShowText(playerText);

            if(interactOnce)
                selfCollider.enabled = false;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        Interact();
    }
}
