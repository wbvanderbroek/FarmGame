using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public float interactDistance = 5f;

    [SerializeField] private InventoryObject inventory;
    [SerializeField] private InventoryObject equipment;
    [SerializeField] private InventoryObject hotbar;

    public InventorySlot currentHotbarSlot;

    [SerializeField] private GameObject selectedSlotIndicator;
    [SerializeField] private GameObject hotbarGO;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private PauseMenu pauseMenuScript;

    private PlayerMovement playerMovement;
    [SerializeField] private ItemObject testobject;
    [SerializeField] private GameObject dropItem;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentHotbarSlot = hotbar.GetSlots[0];
        pauseMenuScript = pauseMenu.transform.parent.GetComponent<PauseMenu>();
    }

    void Update()
    {

        HandleHotbar();
        #region escape menu and inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!pauseMenuScript.IsPaused)
            {
                OpenOrCloseInventory();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuScript.IsPaused)
            {
                HideMouse();
                pauseMenu.SetActive(false);
            }
            else
            {
                ShowMouse();
                pauseMenu.SetActive(true);
            }
            if (inventoryUI.activeInHierarchy)
            {
                OpenOrCloseInventory();//close inventory
            }
        }
        #endregion


        if (Input.GetMouseButtonDown(0) && currentHotbarSlot.item.type == ItemType.Seed && currentHotbarSlot.ItemObject is SeedObject seedObject)
        {
            if (GetComponent<CropPlacement>().PlaceCrop(seedObject.cropObject))
            {
                currentHotbarSlot.amount--;
                currentHotbarSlot.UpdateSlot(currentHotbarSlot.item, currentHotbarSlot.amount);
            }
        }
        Interactions();
    }
    private void Interactions()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("GroundItem") && Input.GetKeyDown(KeyCode.E))
            {
                InventorySlot _slot = hit.collider.transform.GetComponent<GroundItem>().slot;
                InventoryManager.Instance.AddItemToInventories(_slot.item, _slot.amount);
                Destroy(hit.collider.transform.gameObject);
            }


            if (hit.collider.CompareTag("Chest") && Input.GetKeyDown(KeyCode.E) && !InventoryManager.Instance.currentlyOpenedUI)
            {
                if (!pauseMenuScript.IsPaused)
                {
                    InventoryManager.Instance.currentlyOpenedUI = hit.transform.gameObject;
                    InventoryManager.Instance.currentlyOpenedUI.GetComponent<Chest>().OpenChest();
                    OpenOrCloseInventory();
                }
            }
            else if (hit.collider.CompareTag("Shop") && Input.GetKeyDown(KeyCode.E) && !InventoryManager.Instance.currentlyOpenedUI)
            {
                if (!pauseMenuScript.IsPaused)
                {
                    InventoryManager.Instance.currentlyOpenedUI = hit.transform.gameObject;
                    InventoryManager.Instance.currentlyOpenedUI.GetComponent<Shop>().OpenShop();
                    OpenOrCloseInventory();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && InventoryManager.Instance.currentlyOpenedUI)
            {
                OpenOrCloseInventory();
            }


            // Crop detection
            if (hit.collider.CompareTag("Crop") && Input.GetKeyDown(KeyCode.E))
            {
                GameObject currentCrop = hit.collider.gameObject;
                Crop currentCropScript = currentCrop.GetComponent<Crop>();
                if (currentCropScript.stage == Crop.growthStage.HarvestReady && currentCropScript.cropObject is CropObject cropObject)
                {
                    if (InventoryManager.Instance.AddItemToInventories(currentCropScript.cropObject.data, cropObject.amountToHarvest))
                    {
                        currentCrop.GetComponent<Crop>().Harvest();
                    }
                }
            }
        }
    }

    private void HandleHotbar()
    {
        int.TryParse(Input.inputString, out int num);
        KeyCode keyCode = KeyCode.Alpha0 + num;
        if (Input.GetKeyDown(keyCode) && (int)keyCode > 48 && (int)keyCode < 58)
        {
            currentHotbarSlot = hotbar.GetSlots[(int)keyCode - 49];
            selectedSlotIndicator.transform.position = selectedSlotIndicator.transform.parent.
                GetChild((int)keyCode - 49 + 1 /*because there is a indicator now */).transform.position;
        }
    }
    private void OpenOrCloseInventory()
    {
        if (inventoryUI.activeInHierarchy) //close inventory
        {
            if (!pauseMenuScript.IsPaused)
            {
                HideMouse();
            }
            inventoryUI.SetActive(false);
            hotbarGO.GetComponent<Image>().color = new Color(0, 0, 0, 1);

            hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            if (InventoryManager.Instance.currentlyOpenedUI != null)
            {
                if (InventoryManager.Instance.currentlyOpenedUI.TryGetComponent<Shop>(out Shop shop))
                {
                    shop.CloseShop();
                }
                else if (InventoryManager.Instance.currentlyOpenedUI.TryGetComponent<Chest>(out Chest chest))
                {
                    chest.CloseChest();
                }

                InventoryManager.Instance.currentlyOpenedUI = null;
            }
        }
        else //open inventory
        {
            ShowMouse();
            inventoryUI.SetActive(true);
            hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            if (InventoryManager.Instance.currentlyOpenedUI)
            {
                hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -130);
                inventoryUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
            }
            else
            {
                inventoryUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -130);
            }
            hotbarGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        }
    }
    private void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.canMove = true;
    }
    private void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.canMove = false;
    }
    public void DropItems(InventorySlot _slot)
    {
        GameObject droppedItem = Instantiate(dropItem, transform.position, Quaternion.identity);
        droppedItem.GetComponent<GroundItem>().slot.item = _slot.item;
        droppedItem.GetComponent<GroundItem>().slot.amount = _slot.amount;

        droppedItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _slot.ItemObject.icon;
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
        hotbar.Container.Clear();
    }
}
