using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected int totalHealth;
    [SerializeField] protected bool isInvulnerable = false;
    protected bool isDead = false;
    public bool IsDead
    {
        get {return isDead;}
    }
    public bool IsInvulnerable
    {
        set {isInvulnerable = value;}
    }

    protected int currentHealth;

    public virtual bool CheckIfHealthFull()
    {
        return currentHealth == totalHealth ? true : false;
    }

    public virtual void TakeDamage(int damage)
    {
        if(!isInvulnerable)
            currentHealth -= damage;

        if(currentHealth <= 0)
        {
            if(currentHealth < 0)
                currentHealth = 0;

            Death();
        }
    }

    public virtual void TakeHealing(int heal)
    {
        currentHealth += heal;

        if(currentHealth > totalHealth)
            currentHealth = totalHealth;
    }

    protected virtual void Start()
    {
        currentHealth = totalHealth;
    }

    protected virtual void Death()
    {
        isDead = true;
        //Destroy(gameObject);
    }
}
