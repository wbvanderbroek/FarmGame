using UnityEngine;

public class SlamAttackObject : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 moveDirection;
    private int damage = 10;
    private float timeTolive = 15f;
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
    void Update()
    {
        if (timeTolive > 0)
        {
            timeTolive -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        transform.position += moveDirection * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCombat playerCombat))
        {
            playerCombat.TakeDamage(damage);
        }
        if (!other.gameObject.TryGetComponent(out SlamAttackObject slamAttackObject) && !other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Destroy(gameObject);
        }
    }

}
