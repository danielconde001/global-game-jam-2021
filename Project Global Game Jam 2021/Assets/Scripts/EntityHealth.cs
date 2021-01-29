using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected int totalHealth;
    [SerializeField] protected bool isInvulnerable = false;

    protected int currentHealth;

    public virtual void TakeDamage(int damage)
    {
        if(isInvulnerable)
            currentHealth -= damage;

        if(currentHealth <= 0)
        {
            if(currentHealth < 0)
                currentHealth = 0;

            Death();
        }
    }

    protected virtual void Start()
    {
        currentHealth = totalHealth;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
