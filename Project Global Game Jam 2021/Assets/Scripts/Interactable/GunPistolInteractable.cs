using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistolInteractable : Interactable
{
    public override void Interact()
    {
        base.Interact();

        HandgunScriptLPFP.current.UnholsterGun();
        Destroy(gameObject);
    }
}
