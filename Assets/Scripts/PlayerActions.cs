using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public float interactDistance = 5f;
    public string cropTag = "Crop";

    public InventoryObject inventory;

    private Vector3 lastPosition;

    public LayerMask placementLayerMask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //inventory
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
                    inventory.AddItem(currentCropScript.cropObject, 1);
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
        //inventory.Container.Clear();
    }
}
