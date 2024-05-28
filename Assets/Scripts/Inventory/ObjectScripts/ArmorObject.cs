using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory/Armor")]
public class ArmorObject : ItemObject
{
    public int defense = 5;
    public void Awake()
    {
        type = ItemType.Helmet;
    }
}