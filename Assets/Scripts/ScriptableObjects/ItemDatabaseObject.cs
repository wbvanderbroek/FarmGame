using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName = "Inventory/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        GetItem = new Dictionary<int, ItemObject>();

        for (int i = 0; i < Items.Length; i++)
        {
            try
            {
                Items[i].data.Id = i;
                GetItem.Add(i, Items[i]);
            }
            catch
            {
                Debug.LogWarning("OnAfterDeserialze");
            }
        }
    }
    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}
