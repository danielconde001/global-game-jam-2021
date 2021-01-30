using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;

    public float health;

    [SerializeField] Animator animator;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [SerializeField] List<Transform> waypoints;
    [SerializeField] float minIdleTime;
    [SerializeField] float maxIdleTime;
    private float idleTime = 0;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet && idleTime <= 0)
        {
            animator.SetInteger("AnimState", 1);
            agent.SetDestination(walkPoint);
        }

        idleTime -= Time.deltaTime;

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            animator.SetInteger("AnimState", 0);
            idleTime = Random.Range(minIdleTime, maxIdleTime);
        }
    }
    private void SearchWalkPoint()
    {
        walkPoint = waypoints[Random.Range(0, waypoints.Count)].position;
        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        animator.SetInteger("AnimState", 1);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        Quaternion rotation = Quaternion.LookRotation((player.position - transform.position).normalized, Vector3.up);
        
        transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(
            transform.eulerAngles.x,
            rotation.eulerAngles.y,
            transform.eulerAngles.z), 2);

        if (!alreadyAttacked)
        {
            ///Attack code here
            animator.SetTrigger("Attack");
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
