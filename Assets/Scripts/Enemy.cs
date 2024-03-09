using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    private float health = 25;
    private PlayerCombat playerCombat;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange;
    private bool walkPointSet;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;
    private int damage = 10;
    public ItemObject weaponObject;
    [SerializeField] private GameObject handObject;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
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
        handObject.GetComponent<MeshFilter>().sharedMesh = null;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        handObject.GetComponent<MeshFilter>().sharedMesh = null;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (weaponObject.data.Id > -1)
        {
            handObject.GetComponent<MeshFilter>().sharedMesh = weaponObject.model.GetComponent<MeshFilter>().sharedMesh;
            handObject.GetComponent<MeshRenderer>().sharedMaterials = weaponObject.model.GetComponent<MeshRenderer>().sharedMaterials;
        }

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            PerformSwordAttack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void PerformSwordAttack()
    {
        playerCombat.TakeDamage(damage);
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
