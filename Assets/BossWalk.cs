using UnityEngine;

public class BossWalk : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody rb;
    [SerializeField] private float speed = 0.2f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.transform.parent.GetComponent<Boss>().player;
        rb = animator.transform.parent.GetComponent<Rigidbody>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetPos = new Vector3(player.position.x, rb.position.y, player.position.z);

        if (Vector3.Distance(player.position, rb.position) > 10)
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }

        rb.transform.LookAt(targetPos);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
