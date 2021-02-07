using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperInteractable : Interactable
{
    [TextArea(3, 6)]
    [SerializeField] private string paperMessage;

    public override void Interact()
    {
        base.Interact();

        if(canInteract)
            PlayerInteractContext.current.ShowPaperText(paperMessage);
    }
}