using UnityEngine;

public class EnemyIdle : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Boss>().player;
        rb = animator.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetPos = new Vector3(player.position.x, rb.position.y, player.position.z);

        rb.transform.LookAt(targetPos);

        if (animator.TryGetComponent<Boss>(out Boss boss))
        {
            if (boss.playerIsInRoom)
            {
                animator.SetTrigger("Fight");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
