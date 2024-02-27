using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private InventoryObject equipment;
    [SerializeField] private InventoryObject hotbar;

    public bool IsPaused
    {
        get
        {
            if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                return true;
            }
            return false;
            
        }
    }
    public void Save()
    {
        inventory.Save();
        equipment.Save();
        hotbar.Save();
    }
    public void Load()
    {
        inventory.Load();
        equipment.Load();
        hotbar.Load();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
