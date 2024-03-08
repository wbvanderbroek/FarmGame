using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private Collider pickaxeHitBoxCol;
    private int damage = 10;
    public void SwingPickaxe()
    {
        Collider[] colliders = Physics.OverlapBox(pickaxeHitBoxCol.bounds.center, pickaxeHitBoxCol.bounds.extents, pickaxeHitBoxCol.transform.rotation, LayerMask.GetMask("Ore"));

        foreach (Collider collider in colliders)
        {
            print("11");

            if (collider.TryGetComponent<Ore>(out var ore))
            {
                ore.TakeDamage(damage);
            }
        }
    }
}
