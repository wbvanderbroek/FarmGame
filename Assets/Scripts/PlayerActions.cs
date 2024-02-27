using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public float interactDistance = 5f;
    public string cropTag = "Crop";

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
        //Hotbar input
        int.TryParse(Input.inputString, out int num);
        KeyCode keyCode = KeyCode.Alpha0 + num;
        if (Input.GetKeyDown(keyCode) && (int)keyCode > 48 && (int)keyCode < 58)
        {
            currentHotbarSlot = hotbar.GetSlots[(int)keyCode - 49];
            selectedSlotIndicator.transform.position = selectedSlotIndicator.transform.parent.
                GetChild((int)keyCode - 49 + 1 /*because there is a indicator now */).transform.position;
        }

        //test to get more stacks
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddItemToInventories(new Item(foodObject), 1);
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
            if (!pauseMenuScript.IsPaused)
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

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("GroundItem") && Input.GetKeyDown(KeyCode.E))
            {
                ItemObject _item = hit.collider.transform.GetComponent<GroundItem>().item;
                AddItemToInventories(_item.data, 1);
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
            else if (hit.collider.CompareTag("Chest") && Input.GetKeyDown(KeyCode.E) && currentlyOpenedChest)
            {
                OpenOrCloseInventory();
            }
            // Crop detection
            if (hit.collider.CompareTag(cropTag) && Input.GetKeyDown(KeyCode.E))
            {
                GameObject currentCrop = hit.collider.gameObject;
                Crop currentCropScript = currentCrop.GetComponent<Crop>();
                if (currentCropScript.stage == Crop.growthStage.HarvestReady) 
                {
                    if (AddItemToInventories(new Item(currentCropScript.cropObject), 1))
                    {
                        currentCrop.GetComponent<Crop>().Harvest();
                    }
                }
            }
        }
    }
    private bool AddItemToInventories(Item _item, int _amount)
    {
        if (hotbar.AddItem(_item, _amount))
        {
            return true;
        }
        else if (inventory.AddItem(_item, _amount))
        {
            return true;
        }
        return false;
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
            hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -130);
            hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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
        dropItem.GetComponent<GroundItem>().item.data = _slot.item;
        print("drop item");
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
