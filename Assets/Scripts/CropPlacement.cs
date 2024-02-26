using UnityEngine;

public class CropPlacement : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private GameObject crop;
    void Update()
    {
        Vector3 mousePosition = playerActions.GetSelectedMapPosition();
        Vector3 roundedPosition = RoundPositionToNearestWholeNumber(mousePosition);

        cellIndicator.transform.position = new Vector3(roundedPosition.x, mousePosition.y + 0.01f, roundedPosition.z);
        if (Input.GetMouseButtonDown(0))// && playerActions.currentHotbarSlot.item.type == ItemType.Crop)
        {
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
                Instantiate(crop, roundedPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 RoundPositionToNearestWholeNumber(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
    }
}
