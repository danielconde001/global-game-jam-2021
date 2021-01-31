using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] protected float hitRadius;
    [SerializeField] protected Vector3 offset;
    [SerializeField] protected bool enableGizmos;
    [SerializeField] protected LayerMask canHit;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public uint AttackID = 0;
    [HideInInspector] public bool AttackWasHit;

    protected void Awake() {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Attack() {
        isAttacking = true;
        AttackWasHit = false;
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        
        if (AttackWasHit)
            AttackID++;
    }

    protected void OnDrawGizmos() {
        if (!enableGizmos) return;
        Gizmos.DrawWireSphere(transform.position + offset, hitRadius);
    }
}
