using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Collider swordHitBoxCol;
    private float health = 25;
    private float maxHealth = 25;
    [SerializeField] private Image healthBar;
    [SerializeField] private InventoryObject equipment;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        healthText.text = health.ToString();
    }
    public void PerformSwordAttack()
    {
        int damage = 0;
        if (GetComponent<PlayerActions>().currentHotbarSlot.ItemObject is WeaponObject weapon)
        {
            damage = weapon.damage;
        }
        swordHitBoxCol.enabled = true;
        Collider[] colliders = Physics.OverlapBox(swordHitBoxCol.bounds.center, swordHitBoxCol.bounds.extents, swordHitBoxCol.transform.rotation, LayerMask.GetMask("Enemy"));
        swordHitBoxCol.enabled = false; 
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        float defenseStat = 0;
        foreach (var slot in equipment.GetSlots)
        {
            if (slot.ItemObject is ArmorObject armor)
            {
                defenseStat += armor.defense;
            }
        }

        defenseStat = defenseStat / 100;
        if (defenseStat >= 100)
        {
            Debug.LogWarning("armor has too much defense player wont take any damage");
        }
        defenseStat = Mathf.Clamp01(defenseStat);
        print(defenseStat);
        damage = damage *  (1 - defenseStat);
        health -= damage;

        if (health <= 0)
        {
            Debug.LogWarning("player has no more hp");
        }
        healthBar.fillAmount = health / maxHealth;
        healthText.text = health.ToString();
    }
}
