using UnityEngine;

[CreateAssetMenu(fileName = "New Ore Object", menuName = "Inventory/Ore")]
public class OreObject : ItemObject
{
    public int amountToHarvest;
    public void Awake()
    {
        type = ItemType.Ore;
    }
}
