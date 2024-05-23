using UnityEngine;
using UnityEngine.AI;

public class EnemyIdle : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Enemy enemy;
    private float sightRange = 15f, attackRange = 10f;
    private bool playerInSightRange, playerInAttackRange;

    //Patroling
    private Vector3 walkPoint;
    private float walkPointRange = 5f; 
    private bool walkPointSet;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        enemy = animator.GetComponent<Enemy>(); 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent<Boss>(out Boss boss))
        {
            if (boss.playerIsInRoom)
            {
                animator.SetTrigger("Fight");
            }
        }
        else//is normal enemy
        {
            if (playerInSightRange) 
            {
                animator.SetTrigger("Fight");
            }
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(navMeshAgent.transform.position, sightRange, enemy.whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(navMeshAgent.transform.position, attackRange, enemy.whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();

            }
        }
    }
    private void Patroling()
    {
        //handObject.GetComponent<MeshFilter>().sharedMesh = null;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            navMeshAgent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = navMeshAgent.transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(navMeshAgent.transform.position.x + randomX, navMeshAgent.transform.position.y, navMeshAgent.transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -navMeshAgent.transform.up, 2f, enemy.whatIsGround))
            walkPointSet = true;
    }
}
