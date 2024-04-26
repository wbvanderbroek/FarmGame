using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.AI.Navigation;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance;
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

    [Header("Possible rooms to spawn rotated")]
    public GameObject endRoom90;
    public GameObject endRoom180;
    public GameObject endRoom270;
    public GameObject room3Doors90;
    public GameObject room3Doors180;
    public GameObject room3Doors270;
    public GameObject room2Doors90;
    public GameObject room2LDoors90;
    public GameObject room2LDoors180;
    public GameObject room2LDoors270;
    [Space(10)]

    [Header("Stuff to spawn inside rooms")]
    public GameObject[] orePool;
    public GameObject[] enemyPool;
    public GameObject doorCloser;
    [Space(10)]

    [Header("Grid variables")]
    [SerializeField] private GameObject RoomPosPrefab;
    [SerializeField] private Vector2 gridSize = new Vector2(50,50);
    public Dictionary<Vector3, GameObject> roomPositions = new Dictionary<Vector3, GameObject>();
    public Dictionary<int, Vector3> allDoorPositions = new Dictionary<int, Vector3>();

    public Stopwatch stopwatch =  new Stopwatch ();
    private Vector3 defaultRoomSize = new Vector3(40,10,40);
    private bool done = false;
    public event Action OnDone;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //stopwatch.Start();
        Vector3 startingPosition = transform.position - new Vector3((gridSize.x / 2) * defaultRoomSize.x,
            0, (gridSize.y / 2) * defaultRoomSize.z);
        int amountSpawned = 0;
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector3 position = startingPosition + new Vector3(x * defaultRoomSize.x,
                    0, y * defaultRoomSize.z);
                Collider[] checkCollider = Physics.OverlapBox(position, defaultRoomSize / 1.9f, Quaternion.identity);

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
    private void Update()
    {
        if (roomsLeftToSpawn <= 0 && !done)
        {
            //stopwatch.Stop();
            //print(stopwatch.ElapsedMilliseconds);

            Invoke(nameof(BakeNavMesh), 2f);
            done = true;
        }
    }
    private void BakeNavMesh()
    {
        navSurface.BuildNavMesh();
        OnDone?.Invoke();

    }
}
