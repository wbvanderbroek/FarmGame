using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory/Weapon")]
public class WeaponObject : ItemObject
{
    public int damage = 10;
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
