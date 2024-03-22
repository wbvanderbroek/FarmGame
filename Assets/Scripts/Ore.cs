using UnityEngine;

public class Ore : MonoBehaviour
{
    private int oreHealth = 40;
    [SerializeField] private ItemObject oreObject;
    public void TakeDamage(int damage)
    {
        oreHealth = oreHealth - damage;
        if (oreHealth <= 0)
        {
            if (oreObject is OreObject _oreObject)
            {
                InventoryManager.Instance.AddItemToInventories(oreObject.data, _oreObject.amountToHarvest);
                Destroy(gameObject);
            }
        }
    }
}
