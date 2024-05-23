using UnityEngine;
using UnityEngine.AI;

public class EnemyFight : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private float slamAttackCooldown = 5f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        player = animator.GetComponent<Enemy>().player;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(player.position, navMeshAgent.transform.position) > 5)
        {
            ChasePlayer();
        }

        //boss only
        if (animator.TryGetComponent<Boss>(out Boss boss))
        {
            if (slamAttackCooldown > 0)
            {
                slamAttackCooldown -= Time.deltaTime;
            }
            else
            {
                slamAttackCooldown = 5f;
                StartSlamAttackAnimation(animator);
            }
        }
    }
    public void StartNormalAttackAnimation(Animator animator)
    {
        animator.SetTrigger("NormalAttack");
    }
    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
    public void StartSlamAttackAnimation(Animator animator)
    {
        animator.SetTrigger("SlamAttack");
    }
}
