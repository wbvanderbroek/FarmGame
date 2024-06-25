using UnityEngine;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private InventoryObject hotbar;
    //[SerializeField] private InventoryObject equipment;
    [HideInInspector] public GameObject currentlyOpenedUI;
    private void Awake()
    {
        Instance = this;
    }
    public bool AddItemToInventories(Item _item, int _amount)
    {

        if (hotbar.AddItem(_item, _amount))
        {
            return true;
        }
        else if (inventory.AddItem(_item, _amount))
        {
            return true;
        }
        return false;
    }
    public bool FindItemOnInventories(Item _item)
    {
        if (hotbar.FindItemOnInventory(_item, false) != null)
        {
            return true;
        }
        else if (inventory.FindItemOnInventory(_item, false) != null)
        {
            return true;
        }
        return false;
    }
    public bool RemoveItemFromInventories(Item _item)
    {
        if (hotbar.RemoveItem(_item))
        {
            return true;
        }
        if (inventory.RemoveItem(_item))
        {
            return true;
        }
        return false;
    }
    public void RemoveRandomItem()
    {
        if (hotbar.GetAllItems().Count > 0)
        {
            int rnd = Random.Range(0, hotbar.GetAllItems().Count);
            hotbar.RemoveItem(hotbar.GetAllItems()[rnd]);
        }

    }
}
