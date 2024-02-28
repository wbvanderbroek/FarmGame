using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public InventorySlot slot;
    private void LateUpdate()
    {
        transform.GetChild(0).transform.forward = Camera.main.transform.forward;
    }
}
