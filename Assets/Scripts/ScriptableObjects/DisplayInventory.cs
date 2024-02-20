using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public int XStart;
    public int YStart;
    public int XSpaceBetweenItem;
    public int YSpaceBetweenItem;
    public int NumberOfColumn;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject,InventorySlot>();
    private void Start()
    {
        //CreateDisplay();
        CreateSlots();

    }
    private void Update()
    {
        UpdateSlots();
        //UpdateDisplay();
    }
    //public void CreateDisplay()
    //{
    //    for (int i = 0; i < inventory.Container.Items.Count; i++)
    //    {
    //        InventorySlot slot = inventory.Container.Items[i];
    //        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
    //        obj.transform.GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.Id].icon;
    //        obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
    //        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
    //        itemsDisplayed.Add(slot, obj);
    //    }
    //}
    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            print(_slot.Value.ID);
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetComponent<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].icon;
                _slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                //_slot.Key.transform.GetComponent<Image>().sprite = null;
                _slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
    public void CreateSlots()
    {
        itemsDisplayed =  new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }
    public Vector3 GetPosition(int i)
    {
        int column = i % NumberOfColumn;
        int row = i / NumberOfColumn;
        return new Vector3(XStart + (XSpaceBetweenItem * column), YStart - (YSpaceBetweenItem * row), 0f);
    }

    //public void UpdateDisplay()
    //{
    //    for (int i = 0; i < inventory.Container.Items.Count; i++)
    //    {
    //        InventorySlot slot = inventory.Container.Items[i];

    //        if (itemsDisplayed.ContainsKey(slot))
    //        {
    //            itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
    //        }
    //        else
    //        {
    //            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
    //            obj.transform.GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.Id].icon;
    //            obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
    //            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
    //            itemsDisplayed.Add(slot, obj);
    //        }
    //    }
    //}
}
