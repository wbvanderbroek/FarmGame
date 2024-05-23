using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    private float health = 25;

    //Attacking
    public ItemObject weaponObject;
    [SerializeField] private GameObject handObject;
    [SerializeField] private float speed = 3.5f;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (weaponObject.data.Id > -1)
        {
            handObject.GetComponent<MeshFilter>().sharedMesh = weaponObject.model.GetComponent<MeshFilter>().sharedMesh;
            handObject.GetComponent<MeshRenderer>().sharedMaterials = weaponObject.model.GetComponent<MeshRenderer>().sharedMaterials;
        }
        player = GameObject.Find("===Player===").transform;
        navMeshAgent.speed = speed;

    }
    private void Update()
    {
        Vector3 targetRotation = new Vector3(player.position.x, transform.position.y, player.position.z);
        navMeshAgent.transform.LookAt(targetRotation);
    }
    //triggered within animation
    private void NormalAttack()
    {
        //implement damage check
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
