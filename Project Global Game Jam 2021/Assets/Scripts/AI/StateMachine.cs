using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] Transform playerTransform;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    void Update()
    {
        
    }
}
