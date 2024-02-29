using UnityEngine;

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


    [SerializeField] private LayerMask placementLayerMask;
    private Vector3 lastPosition;

    private GameObject currentlyOpenedChest;
    private PlayerMovement playerMovement;
    [SerializeField] private FoodObject foodObject;
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

        //test to get more stacks
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InventoryManager.Instance.AddItemToInventories(new Item(foodObject), 1);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        { 
            if(!pauseMenuScript.IsPaused)
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


            if (hit.collider.CompareTag("Chest") && Input.GetKeyDown(KeyCode.E) && !currentlyOpenedChest)
            {
                if (!pauseMenuScript.IsPaused)
                {
                    currentlyOpenedChest = hit.transform.gameObject;
                    currentlyOpenedChest.GetComponent<Chest>().OpenChest();
                    OpenOrCloseInventory();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && currentlyOpenedChest)
            {
                OpenOrCloseInventory();
            }


            // Crop detection
            if (hit.collider.CompareTag("Crop") && Input.GetKeyDown(KeyCode.E))
            {
                GameObject currentCrop = hit.collider.gameObject;
                Crop currentCropScript = currentCrop.GetComponent<Crop>();
                if (currentCropScript.stage == Crop.growthStage.HarvestReady)
                {
                    if (InventoryManager.Instance.AddItemToInventories(new Item(currentCropScript.cropObject), 2))
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

            hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            if (currentlyOpenedChest != null)
            {
                currentlyOpenedChest.GetComponent<Chest>().CloseChest();
                currentlyOpenedChest = null;
            }
        }
        else //open inventory
        {
            ShowMouse();
            inventoryUI.SetActive(true);
            hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            if (currentlyOpenedChest)
            {
                hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -130);
                inventoryUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);


            }
            else
            {
                inventoryUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -130);

            }


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
        print(droppedItem);
        droppedItem.GetComponent<GroundItem>().slot.item = _slot.item;
        droppedItem.GetComponent<GroundItem>().slot.amount = _slot.amount;

        droppedItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _slot.ItemObject.icon;
    }
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Camera.main.transform.forward;
        mousePos.y = Camera.main.nearClipPlane;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactDistance, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
        hotbar.Container.Clear();
    }
}
