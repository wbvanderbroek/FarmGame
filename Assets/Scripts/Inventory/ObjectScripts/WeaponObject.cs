using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory/Weapon")]
public class WeaponObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
