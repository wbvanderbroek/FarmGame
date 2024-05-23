using UnityEngine;

public class SlamAttackObject : MonoBehaviour
{
    public float speed = 5f;  // Speed at which the object will move
    private Vector3 moveDirection;
    private int damage = 10;
    // This method is called to set the direction in which the object should move
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    void Update()
    {
        // Move the object in the specified direction
        transform.position += moveDirection * speed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerCombat playerCombat))
        {
            playerCombat.TakeDamage(damage);
        }
        print(collision.gameObject.name);
        Destroy(gameObject); 
    }
}
