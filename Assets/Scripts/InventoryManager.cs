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
}
