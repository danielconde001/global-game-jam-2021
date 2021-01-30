using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] EnemyAttack hitBoxOwner;
    uint CurrentAttackID = 0;

    PlayerHealth playerHealth;

    private void Awake() 
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();    
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player")
        {
            if (hitBoxOwner.isAttacking && (hitBoxOwner.AttackID == CurrentAttackID))
            {
                CurrentAttackID++;
                playerHealth.TakeDamage(25);
            }
            hitBoxOwner.AttackWasHit = true;
        }
    }
}
