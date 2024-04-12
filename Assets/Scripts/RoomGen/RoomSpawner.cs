using Unity.AI.Navigation;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public static RoomSpawner Instance;
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

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        GameObject _startRoom = Instantiate(startRoom, transform.position, Quaternion.identity);
        _startRoom.GetComponent<Room>().SpawnRooms();
        //Invoke(nameof(BakeNavMesh), 18.5f);
    }
    private void BakeNavMesh()
    {
        navSurface.BuildNavMesh();
    }
}
