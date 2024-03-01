using UnityEngine;

public class CropPlacement : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private PlayerActions playerActions;
    private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayerMask;


    void Update()
    {
        Vector3 mousePosition = GetSelectedMapPosition();
        Vector3 roundedPosition = RoundPositionToNearestWholeNumber(mousePosition);
        cellIndicator.transform.position = new Vector3(roundedPosition.x, mousePosition.y + 0.01f, roundedPosition.z);

    }
    public bool PlaceCrop(GameObject _crop)
    {
        Vector3 mousePosition = GetSelectedMapPosition();
        Vector3 roundedPosition = RoundPositionToNearestWholeNumber(mousePosition);

        Collider[] colliders = Physics.OverlapSphere(roundedPosition, 0.1f);
        bool canPlaceCrop = true;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Crop"))
            {
                canPlaceCrop = false;
                break;
            }
        }

        if (canPlaceCrop)
        {
            Instantiate(_crop, roundedPosition, Quaternion.identity);
            return true;
        }
        return false;
    }

    private Vector3 RoundPositionToNearestWholeNumber(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
    }
    private Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Camera.main.transform.forward;
        mousePos.y = Camera.main.nearClipPlane;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, playerActions.interactDistance, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
