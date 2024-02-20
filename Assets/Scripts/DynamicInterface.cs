using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DynamicInterface : UserInterface
{
    public GameObject inventorySlotPrefab;

    public int XStart;
    public int YStart;
    public int XSpaceBetweenItem;
    public int YSpaceBetweenItem;
    public int NumberOfColumn;

    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        int column = i % NumberOfColumn;
        int row = i / NumberOfColumn;
        return new Vector3(XStart + (XSpaceBetweenItem * column), YStart - (YSpaceBetweenItem * row), 0f);
    }
}
