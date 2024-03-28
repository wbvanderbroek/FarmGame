using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
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

    private void Start()
    {
        Instance = this;
        GameObject _startRoom = Instantiate(startRoom, transform.position, Quaternion.identity);
        StartCoroutine(_startRoom.GetComponent<Room>().SpawnRooms());
        Time.timeScale = 7.5f;
        Invoke(nameof(BakeNavMesh), 18.5f);
    }
    private void BakeNavMesh()
    {
        navSurface.BuildNavMesh();
        Time.timeScale = 1f;
    }
}
