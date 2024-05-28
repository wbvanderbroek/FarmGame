using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private Collider pickaxeHitBoxCol;
    public void SwingPickaxe()
    {
        int damage = 0;
        if (GetComponent<PlayerActions>().currentHotbarSlot.ItemObject is PickaxeObject pickaxe)
        {
            damage = pickaxe.damage;
        }
        pickaxeHitBoxCol.enabled = true;

        Collider[] colliders = Physics.OverlapBox(pickaxeHitBoxCol.bounds.center, pickaxeHitBoxCol.bounds.extents, pickaxeHitBoxCol.transform.rotation, LayerMask.GetMask("Ore"));
        pickaxeHitBoxCol.enabled = false;

        foreach (Collider collider in colliders)
        {

            if (collider.TryGetComponent<Ore>(out var ore))
            {
                ore.TakeDamage(damage);
            }
        }
    }
}
