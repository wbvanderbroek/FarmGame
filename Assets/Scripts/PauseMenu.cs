using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private InventoryObject equipment;
    [SerializeField] private InventoryObject hotbar;
    private Button button;

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
        EconomyManager.Instance.economyObject.Save();
    }
    public void Load()
    {
        inventory.Load();
        equipment.Load();
        hotbar.Load();
        EconomyManager.Instance.economyObject.Load();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
