using UnityEngine;

public class Boss : MonoBehaviour
{
    private float health = 1000;
    private float SlamAttackCooldown = 5f;
    [HideInInspector] public Transform player;
    private Animator animator;
    [SerializeField] private GameObject slamAttackObject;
    public ItemObject weaponObject;
    [SerializeField] private GameObject handObject;
    public bool playerIsInRoom = false;

    void Start()
    {
        if (weaponObject.data.Id > -1)
        {
            handObject.GetComponent<MeshFilter>().sharedMesh = weaponObject.model.GetComponent<MeshFilter>().sharedMesh;
            handObject.GetComponent<MeshRenderer>().sharedMaterials = weaponObject.model.GetComponent<MeshRenderer>().sharedMaterials;
        }
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameObject.Find("===Player===").transform;
    }
    private void Update()
    {

        if (SlamAttackCooldown > 0)
        {
            SlamAttackCooldown -= Time.deltaTime;
        }
        else
        {
            SlamAttackCooldown = 5f;
            StartSlamAttackAnimation();
        }
    }
    public void NormalAttack()
    {
        //handle damaging player
    }
    public void SlamAttack()
    {
        // Spawn slamAttackObject in a ring around the Boss
        int numberOfObjects = 25; // Number of objects to spawn
        float radius = 4.5f; // Radius of the ring

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate the angle for each object
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                0 + 0.5f, // Assuming you want to spawn the objects at the same height as the Boss
                Mathf.Sin(angle) * radius
            );

            // Convert local position to world position
            spawnPosition += transform.position;

            // Instantiate the slamAttackObject at the calculated position
            GameObject spawnedObject = Instantiate(slamAttackObject, spawnPosition, Quaternion.identity);

            // Calculate the direction from the Boss to the spawn position
            Vector3 direction = spawnPosition - transform.position;

            // Initialize the spawned object with the movement direction
            spawnedObject.GetComponent<SlamAttackObject>().Initialize(direction);
        }

        //handle damaging player like spawning particles 
    }
    public void StartNormalAttackAnimation()
    {
        animator.SetTrigger("NormalAttack");
    }
    public void StartSlamAttackAnimation()
    {
        animator.SetTrigger("SlamAttack");
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyBoss), 0.5f);
    }
    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}
