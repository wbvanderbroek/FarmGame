using System.Collections;
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

    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject attackProjectile;

    //Attacking
    public WeaponObject weaponObject;
    [SerializeField] public GameObject handObject;
    [SerializeField] private float speed = 3.5f;
    public bool isMelee = true;
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
        if (isMelee)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsPlayer);
            if (hitColliders.Length > 0)
            {
                hitColliders[0].GetComponent<PlayerCombat>().TakeDamage(weaponObject.damage);
            }
        }
        else
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            GameObject spawnedObject = Instantiate(attackProjectile, spawnPosition, Quaternion.identity);

            Vector3 direction = transform.forward;
            spawnedObject.GetComponent<AttackProjectile>().Initialize(direction);
        }
    }
    public void TakeDamage(int damage)
    {
        StartCoroutine(DamageParticle());
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        if (TryGetComponent(out Boss boss))
        {
            boss.UpdateHealth();
        }
    }
    private IEnumerator DamageParticle()
    {
        Vector3 pos = new Vector3(0, 1.5f, 0);
        GameObject particle = Instantiate(damageParticle, transform.position + pos, Quaternion.identity, transform);
        particle.transform.localScale = new Vector3(3, 3, 3);
        float elapsedTime = 0f;
        float duration = 1f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Destroy(particle);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
