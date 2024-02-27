using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;
    public void OnAfterDeserialize(){}
    public void OnBeforeSerialize()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.icon;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
    }
    private void LateUpdate()
    {
        transform.GetChild(0).transform.forward = Camera.main.transform.forward;
    }
}
