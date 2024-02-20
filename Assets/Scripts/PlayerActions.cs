using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerActions : MonoBehaviour
{
    public float interactDistance = 5f;
    public string cropTag = "Crop";

    public InventoryObject inventory;

    private Vector3 lastPosition;

    public LayerMask placementLayerMask;
    public GameObject inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(false);
            }
            else if (!inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.Load();
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
                    currentCrop.GetComponent<Crop>().Harvest();
                    inventory.AddItem(new Item(currentCropScript.cropObject), 1);
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
        inventory.Container.Items = new InventorySlot[72];
    }
}
