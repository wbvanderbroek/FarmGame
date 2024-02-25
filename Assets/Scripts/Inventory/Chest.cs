using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public InventoryObject chest;
    public GameObject chestUI;
    public int ChestID = 0;
    public void OpenChest()
    {
        chest.Load(ChestID);
        chestUI.SetActive(true);
    }
    public void CloseChest()
    {
        chest.Save(ChestID);
        chestUI.SetActive(false);
    }
    private void Start()
    {
        chest.CreateFile(ChestID);
    }
}
