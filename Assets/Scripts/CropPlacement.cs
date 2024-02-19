using UnityEngine;

public class CropPlacement : MonoBehaviour
{
    [SerializeField] GameObject cellIndicator;
    [SerializeField] PlayerActions playerActions;
    [SerializeField] Grid grid;
    [SerializeField] GameObject crop;

    void Update()
    {
        Vector3 mousePosition = playerActions.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = new Vector3(grid.CellToWorld(gridPosition).x, mousePosition.y + 0.01f , grid.CellToWorld(gridPosition).z);  

        if (Input.GetMouseButtonDown(0))
        {
            Collider[] colliders = Physics.OverlapSphere(grid.CellToWorld(gridPosition), 0.1f); // Adjust the sphere radius as needed
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
                Instantiate(crop, grid.CellToWorld(gridPosition), Quaternion.identity);
            }
            else
            {
                print("Cannot place crop here, another crop already exists.");
            }
        }
    }
}
