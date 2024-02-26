using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName = "Inventory/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;
    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            try
            {
                ItemObjects[i].data.Id = i;
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
