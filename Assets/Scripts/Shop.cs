using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private InventoryObject hotbar;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject shopUI;
    private Transform shopItemTemplate;
    private List<Transform> shopItemsCreated;

    public ItemObject[] itemObjects;
    public int XStart = -50;
    public int YStart = 50;
    public int XSpaceBetweenItem = 60;
    public int YSpaceBetweenItem = 60;
    public int NumberOfColumn = 9;

    private void Awake()
    {
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }
    public void OpenShop()
    {
        shopItemsCreated = new List<Transform>();
        for (int i = 0; i < itemObjects.Length; i++)
        {
            CreateItemButtons(itemObjects[i].icon, itemObjects[i].name, itemObjects[i].cost, i, itemObjects[i], itemObjects[i].itemsNeeded);
        }
        shopUI.SetActive(true);

    }
    public void CloseShop()
    {
        for (int i = 0;i < shopItemsCreated.Count;i++)
        {
            Destroy(shopItemsCreated[i].gameObject);
        }
        shopUI.SetActive(false);
    }
    private void CreateItemButtons(Sprite itemSprite, string itemName, int itemCost, int positionIndex, ItemObject itemObject, ItemObject[] itemsNeeded = null)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemsCreated.Add(shopItemTransform);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        //float shopItemHeight = 50f;
        //shopItemRectTransform.anchoredPosition = new Vector2(0, shopItemTemplate.GetComponent<RectTransform>().anchoredPosition.y - (shopItemHeight * positionIndex));
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = itemName;
        shopItemTransform.Find("priceText").GetComponent<TextMeshProUGUI>().text = itemCost.ToString();
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
        if (itemsNeeded.Length > 0)
        {
            for (int i = 0; i < itemsNeeded.Length; i++)
            {
                var obj = Instantiate(shopItemTransform.Find("itemNeeded"), Vector3.zero, Quaternion.identity, shopItemTransform.Find("itemNeeded").parent);
                obj.GetComponent<Image>().sprite = itemsNeeded[i].icon;
                obj.GetComponent<RectTransform>().position = new Vector2(shopItemTransform.Find("itemNeeded").position.x - (i * 40), shopItemTransform.Find("itemNeeded").position.y);
            }
        }

        shopItemTransform.gameObject.SetActive(true);
        shopItemTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            TryBuyItem(itemObject, itemsNeeded);
        });
    }
    private void TryBuyItem(ItemObject _itemObject, ItemObject[] _itemsNeeded = null)
    {
        if (_itemsNeeded.Length == 0 && EconomyManager.Instance.RemoveCoins(_itemObject.cost))
        {
            InventoryManager.Instance.AddItemToInventories(_itemObject.data, 1);
        }
        else if (_itemsNeeded.Length > 0)
        {
            foreach (ItemObject _item in _itemsNeeded)
            {
                if (!InventoryManager.Instance.FindItemOnInventories(_item.data))
                {
                    return;
                }
            }
            foreach (ItemObject _item in _itemsNeeded)
            {
                InventoryManager.Instance.RemoveItemFromInventories(_item.data);
            }
            InventoryManager.Instance.AddItemToInventories(_itemObject.data, 1);
        }
    }
}
