using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public int quantity;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
}

public enum ItemType
{
    Default,
    Weapon,
    Armor,
    Crop,
    Seed,
    Food
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
    }
}

