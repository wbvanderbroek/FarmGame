using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName = "Inventory/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            try
            {
                Items[i].data.Id = i;
                //GetItem.Add(i, Items[i]);
            }
            catch
            {
                Debug.LogWarning("OnAfterDeserialze");
            }
        }
    }
    public void OnAfterDeserialize()
    {
        UpdateID();
    }
    public void OnBeforeSerialize()
    {
    }
}
