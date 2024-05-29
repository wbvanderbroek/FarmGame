using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory/Food")]
public class FoodObject : ItemObject
{
    public int healAmount = 4;
    public void Awake()
    {
        type = ItemType.Food;
    }
}
