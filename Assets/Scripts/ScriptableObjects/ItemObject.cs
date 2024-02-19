using UnityEngine;
public enum ItemType
{
    Default,
    Weapon,
    Armor,
    Crop,
    Seed,
    Food
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public int quantity;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
}


