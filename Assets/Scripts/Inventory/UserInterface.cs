using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public PlayerActions playerActions;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    private bool splittingStack = false;
    private void Start()
    {
        CreateSlots();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
            OnSlotUpdate(inventory.GetSlots[i]);
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.icon;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
    public abstract void CreateSlots();
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    private void SellItems(GameObject obj)
    {
        if (!InventoryManager.Instance.currentlyOpenedUI)
            return;
        if (InventoryManager.Instance.currentlyOpenedUI.TryGetComponent<Shop>(out Shop shop) && slotsOnInterface[obj].item.Id >= 0)
        {
            EconomyManager.Instance.AddCoins(slotsOnInterface[obj].ItemObject.cost * slotsOnInterface[obj].amount);
            slotsOnInterface[obj].RemoveItem();
        }
    }
    public void OnClick(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
        SellItems(obj);
    }
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        splittingStack = false;
        if (MouseData.tempItemBeingDragged)
            return;
        if (Input.GetMouseButton(1))
        {
            print("right mouse down");
            splittingStack = true;
        }
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var recTrans = tempItem.AddComponent<RectTransform>();
            recTrans.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);

            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.icon;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null )
        {
            playerActions.DropItems(slotsOnInterface[obj]);
            slotsOnInterface[obj].RemoveItem();
            splittingStack = false;
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            if (slotsOnInterface[obj] == mouseHoverSlotData)
            {
                return;
            }

            if (splittingStack && slotsOnInterface[obj].item.Id >= 0 && slotsOnInterface[obj].amount > 1)
            {
                inventory.SplitItems(slotsOnInterface[obj], mouseHoverSlotData);
                return;
            }

            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
}
public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}
