using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;
    public void OnAfterDeserialize(){}
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = item.icon;
        UnityEditor.EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif    

    }
    private void LateUpdate()
    {
        transform.GetChild(0).transform.forward = Camera.main.transform.forward;
    }
}
