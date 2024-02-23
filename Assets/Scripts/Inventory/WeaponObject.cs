using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory/Weapon")]
public class WeaponObject : ItemObject
{
    public float atkBonus;
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
