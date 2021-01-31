using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitInteractable : Interactable
{
    [SerializeField] protected string healthFullText;
    [SerializeField] protected int healAmount;

    public override void Interact()
    {
        base.Interact();

        if(!PlayerHealth.current.CheckIfHealthFull())
        {
            PlayerHealth.current.TakeHealing(healAmount);
            Destroy(gameObject);
        }
        else
        {
            PlayerInteractContext.current.ShowText(healthFullText);
        }
    }
}
