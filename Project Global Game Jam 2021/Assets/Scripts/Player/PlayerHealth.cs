using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : EntityHealth
{
    [SerializeField] private TextMeshProUGUI healthText;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        healthText.text = "Health: " + currentHealth;
    }

    protected override void Start()
    {
        base.Start();

        healthText.text = "Health: " + currentHealth;
    }

    protected override void Death()
    {
        //put game over here
    }
}