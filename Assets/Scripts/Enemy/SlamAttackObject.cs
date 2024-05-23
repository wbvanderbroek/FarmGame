using UnityEngine;

public class SlamAttackObject : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 moveDirection;
    private int damage = 10;
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCombat playerCombat))
        {
            playerCombat.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
