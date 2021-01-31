using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableHead : EntityHealth
{
    [SerializeField] protected EnemyHealth headOwner;
    [SerializeField] protected float damageMultiplier;

    protected override void Start() {
        base.Start();
        if (!headOwner) headOwner = GetComponentInParent<EnemyHealth>();
    }

    public override void TakeDamage(int damage)
    {
        damage = (int)(damage * damageMultiplier);
        headOwner.TakeDamage(damage);
    }

    protected override void Death()
    {
        // Nothing
    }
}
