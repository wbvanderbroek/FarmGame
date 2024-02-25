using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public InventoryObject chest;
    public GameObject chestUI;
    private float targetTime = 2f;
    public void OpenChest()
    {
        chest.Load();
        chestUI.SetActive(true);
    }
    public void CloseChest()
    {
        chest.Save();
        chestUI.SetActive(false);
    }
}
