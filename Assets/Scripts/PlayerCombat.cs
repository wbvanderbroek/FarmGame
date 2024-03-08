using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Collider swordHitBoxCol;
    private float health = 25;

    public void PerformSwordAttack(int damage)
    {
        Collider[] colliders = Physics.OverlapBox(swordHitBoxCol.bounds.center, swordHitBoxCol.bounds.extents, swordHitBoxCol.transform.rotation, LayerMask.GetMask("Enemy"));
        
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.LogWarning("player has no more hp");
        }
    }
}
