using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public float interactDistance = 5f;

    [SerializeField] private InventoryObject inventory;
    [SerializeField] private InventoryObject equipment;
    [SerializeField] private InventoryObject hotbar;

    public InventorySlot currentHotbarSlot;
    private int currentHotbarInt;

    [SerializeField] private GameObject selectedSlotIndicator;
    [SerializeField] private GameObject hotbarGO;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private PauseMenu pauseMenuScript;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;


    [SerializeField] private ItemObject testobject;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private GameObject handObject;
    [SerializeField] private Animator animator;
    private void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHotbarSlot = hotbar.GetSlots[0];
        pauseMenuScript = pauseMenu.transform.parent.GetComponent<PauseMenu>();
    }

    void Update()
    {
        if (InventoryManager.Instance.currentlyOpenedUI == null && !inventoryUI.activeInHierarchy)
        {
            HandleHotbar();
        }
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
                if (InventoryManager.Instance.currentlyOpenedUI == null && !inventoryUI.activeInHierarchy)
                {
                    ShowMouse();
                    pauseMenu.SetActive(true);
                }
                else
                {
                    //close whatever inventory is opened
                    OpenOrCloseInventory();
                }

            }
            if (inventoryUI.activeInHierarchy)
            {
                OpenOrCloseInventory();//close inventory
            }
        }
        #endregion
        if (InventoryManager.Instance.currentlyOpenedUI == null && !pauseMenuScript.IsPaused && !inventoryUI.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0) && currentHotbarSlot.item.type == ItemType.Weapon)
            {
                if (currentHotbarSlot.ItemObject is WeaponObject weapon)
                {
                    animator.SetTrigger("UsingHand");
                }
            }
            if (Input.GetMouseButtonDown(0) && currentHotbarSlot.item.type == ItemType.Pickaxe)
            {
                if (currentHotbarSlot.ItemObject is PickaxeObject pickaxe)
                {
                    animator.SetTrigger("UsingHand");
                }
            }
            if (Input.GetMouseButtonDown(0) && currentHotbarSlot.item.type == ItemType.Food)
            {
                if (currentHotbarSlot.ItemObject is FoodObject food)
                {
                    if (playerCombat.Heal(food.healAmount))
                    {
                        currentHotbarSlot.amount--;
                        currentHotbarSlot.UpdateSlot(currentHotbarSlot.item, currentHotbarSlot.amount);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0) && currentHotbarSlot.item.type == ItemType.Seed && currentHotbarSlot.ItemObject is SeedObject seedObject)
            {
                if (GetComponent<CropPlacement>().PlaceCrop(seedObject.cropObject))
                {
                    currentHotbarSlot.amount--;
                    currentHotbarSlot.UpdateSlot(currentHotbarSlot.item, currentHotbarSlot.amount);
                }
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
        if (currentHotbarSlot.item.Id > -1 && currentHotbarSlot.ItemObject.model != null)
        {
            handObject.GetComponent<MeshFilter>().sharedMesh = currentHotbarSlot.ItemObject.model.GetComponent<MeshFilter>().sharedMesh;
            handObject.GetComponent<MeshRenderer>().sharedMaterials = currentHotbarSlot.ItemObject.model.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else
        {
            handObject.GetComponent<MeshFilter>().sharedMesh = null;
        }


        int.TryParse(Input.inputString, out int num);
        KeyCode keyCode = KeyCode.Alpha0 + num;
        if (Input.GetKeyDown(keyCode) && (int)keyCode > 48 && (int)keyCode < 58)
        {
            currentHotbarInt = (int)keyCode - 49;
            currentHotbarSlot = hotbar.GetSlots[(int)keyCode - 49];
            selectedSlotIndicator.transform.position = selectedSlotIndicator.transform.parent.
                GetChild((int)keyCode - 49 + 1 /*because there is a indicator now */).transform.position;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            int scrollDirection = 0;
            if (scrollInput < 0) 
            {
                scrollDirection = 1;
            }
            else if (scrollInput > 0)
            {
                scrollDirection = -1;
            }
            currentHotbarInt = currentHotbarInt + scrollDirection;
            if (currentHotbarInt < 0)
            {
                currentHotbarInt = 8;
            }
            if (currentHotbarInt > 8)
            {
                currentHotbarInt = 0;
            }
            currentHotbarSlot = hotbar.GetSlots[currentHotbarInt];
            selectedSlotIndicator.transform.position = selectedSlotIndicator.transform.parent.
                GetChild(currentHotbarInt + 1 /*because there is a indicator now */).transform.position;
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

            hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -230);
            hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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
            hotbarGO.GetComponent<Image>().color = new Color(0, 0, 0, 0);

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
        EconomyManager.Instance.Clear();
    }
}
