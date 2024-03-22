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
        //StartCoroutine(SpawnRooms());
        GameObject _startRoom = Instantiate(startRoom, transform.position, Quaternion.identity);
        StartCoroutine(_startRoom.GetComponent<Room>().SpawnRooms());
        Invoke(nameof(BakeNavMesh), 18.5f);
    }
    private void BakeNavMesh()
    {
        navSurface.BuildNavMesh();
    }
    //void Start()
    //{
    //    Instantiate(startRoom);
    //    //Vector3 scale = new Vector3(
    //    //    startRoom.GetComponent<Room>().doors[0].position.x * startRoom.GetComponent<Room>().roomScale.x,
    //    //    startRoom.GetComponent<Room>().doors[0].position.y * startRoom.GetComponent<Room>().roomScale.y,
    //    //    startRoom.GetComponent<Room>().doors[0].position.z * startRoom.GetComponent<Room>().roomScale.z);
    //    //scale *= 2;
    //    //Vector3 pos = transform.position + scale;
    //    //Instantiate(roomPrefabs[0], pos, Quaternion.identity);

    //    foreach (var door in startRoom.GetComponent<Room>().doors)
    //    {
    //        Vector3 scale = new Vector3(
    //            door.position.x * startRoom.GetComponent<Room>().roomScale.x,
    //            door.position.y * startRoom.GetComponent<Room>().roomScale.y,
    //            door.position.z * startRoom.GetComponent<Room>().roomScale.z);
    //        scale *= 2;
    //        Vector3 pos = startRoom.transform.position + scale;
    //        GameObject room = Instantiate(roomPrefabs[0], pos, Quaternion.identity);
    //        for (int i = 0; i < 3; i++)
    //        {
    //            foreach (var door2 in room.GetComponent<Room>().doors)
    //            {
    //                Vector3 scale2 = new Vector3(
    //                    door2.position.x * room.GetComponent<Room>().roomScale.x,
    //                    door2.position.y * room.GetComponent<Room>().roomScale.y,
    //                    door2.position.z * room.GetComponent<Room>().roomScale.z);
    //                scale *= 2;
    //                Vector3 pos2 = room.transform.position + scale;
    //                Instantiate(roomPrefabs[0], pos2, Quaternion.identity);

    //            }
    //        }
    //    }
    //}
    //IEnumerator SpawnRooms()
    //{

    //    //foreach (var door in startRoom.GetComponent<Room>().doors)
    //    //{
    //    //    Vector3 scale = new Vector3(
    //    //        door.localPosition.x * startRoom.GetComponent<Room>().roomScale.x,
    //    //        door.localPosition.y * startRoom.GetComponent<Room>().roomScale.y,
    //    //        door.localPosition.z * startRoom.GetComponent<Room>().roomScale.z);
    //    //    scale *= 2;
    //    //    Vector3 pos = startRoom.transform.position + scale;
    //    //    GameObject room = Instantiate(roomPrefabs[0], pos, Quaternion.identity);
    //    //    room.GetComponent<Room>().motherRoom = gameObject;

    //    //    //foreach (var door2 in room.GetComponent<Room>().doors)
    //    //    //{
    //    //    //    Vector3 scale2 = new Vector3(
    //    //    //        door2.localPosition.x * room.GetComponent<Room>().roomScale.x,
    //    //    //        door2.localPosition.y * room.GetComponent<Room>().roomScale.y,
    //    //    //        door2.localPosition.z * room.GetComponent<Room>().roomScale.z);
    //    //    //    scale2 *= 2;
    //    //    //    Vector3 pos2 = room.transform.position + scale2;
    //    //    //    Collider[] colliders = Physics.OverlapBox(pos2, room.GetComponent<BoxCollider>().bounds.extents / 2);
    //    //    //    if (colliders.Length == 0)
    //    //    //    {
    //    //    //        int rnd = Random.Range(0, roomPrefabs.Length);
    //    //    //        Instantiate(roomPrefabs[rnd], pos2, Quaternion.identity);
    //    //    //    }
    //    //    //    else
    //    //    //    {
    //    //    //        print(colliders[0].gameObject.name);
    //    //    //    }
    //    //    //    yield return new WaitForSeconds(1f);
    //    //    //}
    //        yield return new WaitForSeconds(1f);
    //    //}
    //}
}
