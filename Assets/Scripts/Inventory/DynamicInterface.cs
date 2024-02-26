using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DynamicInterface : UserInterface
{
    public GameObject inventorySlotPrefab;

    //default correct ui positions
    public int XStart = -50;
    public int YStart = 50;
    public int XSpaceBetweenItem = 60;
    public int YSpaceBetweenItem= 60;
    public int NumberOfColumn = 9;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventory.GetSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        int column = i % NumberOfColumn;
        int row = i / NumberOfColumn;
        return new Vector3(XStart + (XSpaceBetweenItem * column), YStart - (YSpaceBetweenItem * row), 0f);
    }
}
