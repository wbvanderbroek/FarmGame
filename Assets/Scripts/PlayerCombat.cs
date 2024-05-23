using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Collider swordHitBoxCol;
    private float health = 25;
    private float maxHealth = 25;
    [SerializeField] private Image healthBar;

    public void PerformSwordAttack(int damage)
    {
        swordHitBoxCol.enabled = true;
        Collider[] colliders = Physics.OverlapBox(swordHitBoxCol.bounds.center, swordHitBoxCol.bounds.extents, swordHitBoxCol.transform.rotation, LayerMask.GetMask("Enemy"));
        swordHitBoxCol.enabled = false; 
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
            if (collider.TryGetComponent<Boss>(out var boss))
            {
                boss.TakeDamage(damage);
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
        healthBar.fillAmount = health / maxHealth;

    }
}
