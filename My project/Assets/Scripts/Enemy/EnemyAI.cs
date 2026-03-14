using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public AnimationClip idleClip;
    public AnimationClip walkClip;
    public Animation animation;

    private Renderer rend;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    float visibleTimer = 0f;
    float requiredVisibleTime = 0.4f;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerNoWall;

    private void Awake()
    { 
        animation.AddClip(walkClip, "walk");
        animation.AddClip(idleClip, "idle");
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
        {
            playerNoWall = ((1 << hit.collider.gameObject.layer) & whatIsPlayer) != 0;
        }
        else
        {
            playerNoWall = false;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerNoWall)
            visibleTimer += Time.deltaTime;
        else
            visibleTimer = 0f;

        bool stableLineOfSight = visibleTimer >= requiredVisibleTime;

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();

        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        else if (playerInAttackRange && playerInSightRange && !stableLineOfSight)
            ChasePlayer();

        else if (playerInAttackRange && playerInSightRange && stableLineOfSight)
            AttackPlayer();

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            animation.CrossFade("walk", 0.2f);
            agent.SetDestination(walkPoint);
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        animation.CrossFade("walk", 0.2f);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        animation.CrossFade("idle", 0.2f);
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Napad
            
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 6f, ForceMode.Impulse);
            
            //
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

}
