using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerActions : MonoBehaviour
{
    //guh
    //public MouseItem mouseItem = new MouseItem();

    public float interactDistance = 5f;
    public string cropTag = "Crop";

    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject hotbar;
    public InventorySlot currentHotbarSlot;
    public GameObject selectedSlotIndicator;
    public GameObject hotbarGO;

    private Vector3 lastPosition;

    public LayerMask placementLayerMask;
    public GameObject inventoryUI;
    private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentHotbarSlot = hotbar.GetSlots[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryUI.activeInHierarchy)
            {
                playerMovement.canMove = true;
                inventoryUI.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
                hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                playerMovement.canMove = false;
                inventoryUI.SetActive(true);
                hotbarGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -130);
                hotbarGO.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
        }

        //Hotbar input
        int.TryParse(Input.inputString, out int num);
        KeyCode keyCode = KeyCode.Alpha0 + num;
        if (Input.GetKeyDown(keyCode) && (int)keyCode > 48 && (int)keyCode < 58)
        {
            currentHotbarSlot = hotbar.GetSlots[(int)keyCode - 49];
            selectedSlotIndicator.transform.position = selectedSlotIndicator.transform.parent.
                GetChild((int)keyCode - 49 + 1 /*because there is a indicator now */).transform.position;

            if (currentHotbarSlot.item.Id >= 0)
            {
                print(currentHotbarSlot.item.Name);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
            hotbar.Save();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.Load();
            equipment.Load();
            hotbar.Load();
        }


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactDistance))
        {
            // Crop detection
            if (hit.collider.CompareTag(cropTag) && Input.GetKeyDown(KeyCode.E))
            {
                GameObject currentCrop = hit.collider.gameObject;
                Crop currentCropScript = currentCrop.GetComponent<Crop>();
                if (currentCropScript.stage == Crop.growthStage.HarvestReady) 
                {
                    if (inventory.AddItem(new Item(currentCropScript.cropObject), 1))
                    {
                        currentCrop.GetComponent<Crop>().Harvest();

                    }
                }
            }
        }
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
