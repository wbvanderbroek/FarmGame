using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed Object", menuName = "Inventory/Seed")]
public class SeedObject : ItemObject
{
    public ItemObject cropObject;
    public void Awake()
    {
        type = ItemType.Seed;
    }
}
