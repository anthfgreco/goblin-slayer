using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private LayerMask groundLayer, playerLayer;

    public float currentHealth, maxHealth;

    [Header("Patroling")]
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] private float walkPointRange;

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("States")]
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange, isAlive;

    [Header("Health Bar")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Transform canvasTransform;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        // Check sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        // If enemy is alive, choose one of it's states depending on sight and attack range
        if (isAlive) {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
        // If enemy is dead, play die animation, destroy enemies collider, and destroy enemy after a time delay
        else {
            anim.Play("Die01");
            Destroy(this.GetComponent<BoxCollider>());
            Invoke(nameof(DestroyEnemy), 15f);
        }
    }

    private void LateUpdate() {
        // Keep health bar perpendicular to the camera
        canvasTransform.LookAt(transform.position + Camera.main.transform.forward); 
    }

    private void Patroling()
    {
        anim.Play("Move01");

        // Set walkpoint
        if (!walkPointSet) {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) {
                walkPointSet = true;
            }
        }
        else {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Debug.Log(agent.velocity.magnitude);

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
        if (agent.velocity.magnitude < 0.05f && walkPointSet) {
            Invoke(nameof(ResetPatrol), 5f);
        }
    }

    private void ResetPatrol()
    {
        walkPointSet = false;
    }
  
    private void ChasePlayer()
    {
        anim.Play("Move01");
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer()
    {
        anim.Play("Attack01");
        
        agent.SetDestination(transform.position);

        transform.LookAt(playerTransform);

        if (!alreadyAttacked)
        {
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
        currentHealth -= damage;

        if (currentHealth <= 0) {
            isAlive = false;
            currentHealth = 0;
            GameObject.Find("Controller (right)").GetComponent<RightController>().goblinsKilled += 1;
        }
        
        UpdateHealthBar();
    }
    private void DestroyEnemy()
    {
        
        Destroy(gameObject);
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount =currentHealth / maxHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
