using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory/Food")]
public class FoodObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Food;
    }
}
