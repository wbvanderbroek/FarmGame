using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public bool isStackable = true;
    public Item data = new Item();
    public int cost;
    public GameObject model;
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
    Food,

    None
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemType type;
    public Item()
    {
        Name = "";
        Id = -1;
        type = ItemType.None;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        type = item.type;
    }
}

