using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : EntityHealth
{
    [SerializeField] protected UnityEvent OnDeath; 
    protected Animator animator;
    
    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Death()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        OnDeath.Invoke();
    }
}
