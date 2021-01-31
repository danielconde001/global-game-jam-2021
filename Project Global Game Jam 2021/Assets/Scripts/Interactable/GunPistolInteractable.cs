using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistolInteractable : Interactable
{
    [SerializeField] private int ammoAmount;
    public override void Interact()
    {
        base.Interact();

        if(HandgunScriptLPFP.current.Holstered)
            HandgunScriptLPFP.current.UnholsterGun();
        else
            HandgunScriptLPFP.current.GiveAmmo(ammoAmount);

        Destroy(gameObject);
    }
}
