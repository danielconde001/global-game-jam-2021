using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Death()
    {
        base.Death();
    }
}
