using UnityEngine;

[CreateAssetMenu(fileName = "New Pickaxe Object", menuName = "Inventory/Pickaxe")]
public class PickaxeObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Pickaxe;
    }
}
