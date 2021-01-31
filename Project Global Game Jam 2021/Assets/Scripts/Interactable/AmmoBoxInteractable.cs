using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxInteractable : Interactable
{
    [SerializeField] protected int ammoAmount;

    public override void Interact()
    {
        base.Interact();

        HandgunScriptLPFP.current.GiveAmmo(ammoAmount);
        Destroy(gameObject);
    }
}
