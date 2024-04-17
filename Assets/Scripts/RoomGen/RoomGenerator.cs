using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance;
    public GameObject[] roomPrefabs;
    [SerializeField] private GameObject startingRoom;
    public int roomsLeftToSpawn = 15;
    [SerializeField] private NavMeshSurface navSurface;
    [Space(10)]

    [Header("Possible rooms to spawn")]
    public GameObject endRoom;
    public GameObject room4Doors;
    public GameObject room3Doors;
    public GameObject room2Doors;
    public GameObject room2LDoors;
    [Space(10)]

    [Header("Stuff to spawn inside rooms")]
    public GameObject[] orePool;
    public GameObject[] enemyPool;
    [Space(10)]

    [Header("Grid variables")]
    [SerializeField] private GameObject RoomPosPrefab;
    [SerializeField] private Vector2 gridSize = new Vector2(50,50);
    public Dictionary<Vector3, GameObject> roomPositions = new Dictionary<Vector3, GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Vector3 startingPosition = transform.position - new Vector3((gridSize.x / 2) * room4Doors.GetComponent<BoxCollider>().size.x,
            0, (gridSize.y / 2) * room4Doors.GetComponent<BoxCollider>().size.x);
        int amountSpawned = 0;
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector3 position = startingPosition + new Vector3(x * room4Doors.GetComponent<BoxCollider>().size.x,
                    0, y * room4Doors.GetComponent<BoxCollider>().size.x);
                Collider[] checkCollider = Physics.OverlapBox(position, room4Doors.GetComponent<BoxCollider>().size / 1.9f, Quaternion.identity);

                if (checkCollider.Length == 0)
                {
                    amountSpawned++;
                    Instantiate(RoomPosPrefab, position, Quaternion.identity, transform);
                }
            }
        }
        roomPositions[transform.position].GetComponent<RoomPos>().status = RoomStatus.Completed;
        GameObject startRoom = Instantiate(startingRoom, transform.position, Quaternion.identity);
        roomPositions[transform.position].GetComponent<RoomPos>().roomInPosition = startRoom;
        startRoom.GetComponent<Room>().SpawnRooms();
        //Invoke(nameof(BakeNavMesh), 18.5f);
    }
    private void BakeNavMesh()
    {
        navSurface.BuildNavMesh();
    }
}
