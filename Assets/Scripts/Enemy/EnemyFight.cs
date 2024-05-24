using UnityEngine;
using UnityEngine.AI;

public class EnemyFight : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private readonly float slamAttackCooldown = 5f;
    private float slamAttackTimer = 5f;
    private float attackRange;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        player = animator.GetComponent<Enemy>().player;
        attackRange = navMeshAgent.GetComponent<Enemy>().attackRange;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool triggerdAnAnimation = false;
        //boss only
        if (animator.TryGetComponent<Boss>(out Boss boss))
        {
            if (slamAttackTimer > 0)
            {
                slamAttackTimer -= Time.deltaTime;
            }
            else if (triggerdAnAnimation == false)
            {
                triggerdAnAnimation = true;
                slamAttackTimer = slamAttackCooldown;
                animator.SetTrigger("SlamAttack");
            }
        }

        if (Vector3.Distance(player.position, navMeshAgent.transform.position) > attackRange)
        {
            ChasePlayer();
        }
        else if (triggerdAnAnimation == false)
        {
            triggerdAnAnimation = true;
            navMeshAgent.ResetPath();
            animator.SetTrigger("NormalAttack");
        }
    }
    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
}
