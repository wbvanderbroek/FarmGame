using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName = "Inventory/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    //public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        //GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();

        for (int i = 0; i < Items.Length; i++)
        {
            try
            {
                //GetId.Add(Items[i], i);
                Items[i].Id = i;
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
