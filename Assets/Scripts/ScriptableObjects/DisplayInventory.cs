using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int XStart;
    public int YStart;
    public int XSpaceBetweenItem;
    public int YSpaceBetweenItem;
    public int NumberOfColumn;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        // Set XStart and YStart to position the top-left corner of the grid

        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i); // Use anchored position instead of local position
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }

    public Vector2 GetPosition(int i)
    {
        int column = i % NumberOfColumn;
        int row = i / NumberOfColumn;
        return new Vector2(XStart + (XSpaceBetweenItem * column), YStart - (YSpaceBetweenItem * row)); // Use Vector2 for anchored position
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }
}
