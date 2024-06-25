using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Collider swordHitBoxCol;
    private float health = 25;
    private float maxHealth = 25;
    [SerializeField] private Image healthBar;
    [SerializeField] private InventoryObject equipment;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image damageTakenImage;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Color flashColor = new Color(1, 0, 0, 0.5f);

    [SerializeField] private Animator transition;
    private void Start()
    {
        transition = GameObject.Find("CanvasCrossfade").GetComponent<Animator>();

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
    public bool Heal(float amount)
    {
        if (health < maxHealth)
        {
            health += amount;
            healthBar.fillAmount = health / maxHealth;
            healthText.text = health.ToString();
            return true;
        }
        return false;
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine(FlashDamageOverlay());
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
        damage = damage *  (1 - defenseStat);
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            InventoryManager.Instance.RemoveRandomItem();
            GetComponent<PlayerMovement>().canMove = false;
            StartCoroutine(LoadScene());
        }
        healthBar.fillAmount = health / maxHealth;
        healthText.text = health.ToString();
    }
    private IEnumerator FlashDamageOverlay()
    {
        float elapsedTime = 0f;

        // Fade in
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, flashColor.a, elapsedTime / (flashDuration / 2));
            damageTakenImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(flashColor.a, 0, elapsedTime / (flashDuration / 2));
            damageTakenImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        damageTakenImage.color = Color.clear;
    }
    private IEnumerator LoadScene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().name == "Main")
        {
            SceneManager.LoadScene("Generation");
        }
        if (SceneManager.GetActiveScene().name == "Generation")
        {
            SceneManager.LoadScene("Main");
        }
    }
}
