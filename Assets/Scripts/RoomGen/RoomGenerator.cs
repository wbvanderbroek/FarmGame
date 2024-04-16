using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance;
    public GameObject[] roomPrefabs;
    [SerializeField] private GameObject startRoom;
    public GameObject endRoom;
    public GameObject room4Doors;
    public GameObject room3Doors;
    public GameObject room2Doors;
    public GameObject room2LDoors;
    public GameObject[] orePool;
    public GameObject[] enemyPool;
    [SerializeField] private NavMeshSurface navSurface;
    public int roomsLeftToSpawn = 15;

    [SerializeField] private GameObject RoomPosPrefab;
    [SerializeField] private Vector2 gridSize = new Vector2(50,50);
    public Dictionary<Vector3, GameObject> roomPositions = new Dictionary<Vector3, GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Vector3 startingPosition = transform.position - new Vector3((gridSize.x / 2) * 10, 0, (gridSize.y / 2) * 10);
        int amountSpawned = 0;
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector3 position = startingPosition + new Vector3(x * 10, 0, y * 10);
                Collider[] checkCollider = Physics.OverlapBox(position, room4Doors.GetComponent<BoxCollider>().size / 1.9f, Quaternion.identity);
                // Vector3 position = new Vector3(x * 10, transform.position.y, y * 10); 
                if (checkCollider.Length == 0)
                {
                    amountSpawned++;
                    Instantiate(RoomPosPrefab, position, Quaternion.identity, transform);
                }
            }
        }

        roomPositions[transform.position].GetComponent<RoomPos>().status = RoomStatus.Completed;
        GameObject _startRoom = Instantiate(startRoom, transform.position, Quaternion.identity);
        _startRoom.GetComponent<Room>().SpawnRooms();
        //Invoke(nameof(BakeNavMesh), 18.5f);
    }
    private void BakeNavMesh()
    {
        navSurface.BuildNavMesh();
    }
}
