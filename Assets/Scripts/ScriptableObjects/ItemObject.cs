using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public int quantity;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
    public bool isStackable = true;
    public Item data = new Item();

}

public enum ItemType
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,

    Weapon,

    Crop,
    Seed,
    Food
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        Debug.Log(Name);
    }
}

