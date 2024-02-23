using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Object", menuName = "Inventory/Crop")]
public class CropObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Crop;
    }
}