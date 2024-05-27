using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float maxHealth = 25;
    public float health;
    public float attackRange = 3.5f;

    //Attacking
    public WeaponObject weaponObject;
    [SerializeField] public GameObject handObject;
    [SerializeField] private float speed = 3.5f;
    private void Awake()
    {
        health = maxHealth;
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);
        if (hitColliders.Length > 0 )
        {
            hitColliders[0].GetComponent<PlayerCombat>().TakeDamage(weaponObject.damage);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        if (TryGetComponent(out Boss boss))
        {
            boss.UpdateHealth();
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
