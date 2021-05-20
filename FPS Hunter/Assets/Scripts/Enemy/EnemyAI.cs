using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private SingletonManager _singletonManager;
    
    private NavMeshAgent _navMeshAgent;
    public GameObject target;
    
    public LayerMask groundMask, playerMask;
    
    // patrolling
    public Vector3 walkPoint;
    private bool _walkPointSet;
    public float walkPointRange;
    
    // Attacking
    private float timeBetweenAttacks;
     public bool alreadyAttacked;
    
    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    // Animations
    private Animator _animator;
    
    
    private void Start()
    {   _singletonManager = SingletonManager.Instance;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        target = _singletonManager.players[Random.Range(0, _singletonManager.players.Count)];
        timeBetweenAttacks = _singletonManager.AnimationManager.enemyAnimations[2].length;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    void Patrolling()
    {
        _animator.SetBool("isMoving", true);
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet) _navMeshAgent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        // WalkPoint Reached
        if (distanceToWalkPoint.magnitude < 1) _walkPointSet = false;

    }

    void SearchWalkPoint()
    {
        _animator.SetBool("isMoving", true);

        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            _walkPointSet = true;
    }
    
    void ChasePlayer()
    {
        _animator.SetBool("isMoving", true);

        _navMeshAgent.SetDestination(target.transform.position);
    }

    void AttackPlayer()
    {
        _navMeshAgent.SetDestination(transform.position);
        transform.LookAt(target.transform);

        if (!alreadyAttacked)
        {
            // Attack
            _animator.SetTrigger("isAttacking");
            
            
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks + .2f);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    


}
