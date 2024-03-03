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
            CreateItemButtons(itemObjects[i].icon, itemObjects[i].name, itemObjects[i].cost, i, itemObjects[i]);
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
    private void CreateItemButtons(Sprite itemSprite, string itemName, int itemCost, int positionIndex, ItemObject itemObject)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemsCreated.Add(shopItemTransform);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        //float shopItemHeight = 50f;
        //shopItemRectTransform.anchoredPosition = new Vector2(0, shopItemTemplate.GetComponent<RectTransform>().anchoredPosition.y - (shopItemHeight * positionIndex));
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = itemName;
        shopItemTransform.Find("priceText").GetComponent<TextMeshProUGUI>().text = itemCost.ToString();
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
        shopItemTransform.gameObject.SetActive(true);
        shopItemTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            TryBuyItem(itemObject);
        });
    }
    private void TryBuyItem(ItemObject _itemObject)
    {
        if (EconomyManager.Instance.RemoveCoins(_itemObject.cost))
        {
            InventoryManager.Instance.AddItemToInventories(_itemObject.data, 1);
        }
    }
}
