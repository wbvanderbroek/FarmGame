using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] doors;
    public Dictionary<Vector3, GameObject> doorPositions = new Dictionary<Vector3, GameObject>();
    public Transform[] notDoors;
    public Transform[] oreSpawnPoints;
    public Transform[] enemySpawnPoints;
    public List<Vector3> doorsAroundTheNextRoom = new List<Vector3>();

    public Vector3 roomScale = new Vector3(1, 1, 1);
    private RoomGenerator roomSpawner;

    public bool AllowRoomSpawn = true;
    public bool triedReplace;
    public bool replaced;
    public int doorsaround;
    public float spawnChance = 0.5f;

    private void Awake()
    {
        foreach (var door in doors)
        {
            doorPositions.Add(door.transform.position, door.gameObject);
        }
    }
    public void UpdateDictionary()
    {
        doorPositions.Clear();
        foreach (var door in doors)
        {
            doorPositions.Add(door.transform.position, door.gameObject);
        }
    }
    //problem rn is that there is no delay making 1 giant snake
    //basicly because only 1 door in starter room will make spawns and only when that one is done it will try other doors
    public void SpawnRooms()
    {
        if (AllowRoomSpawn)
        {
            roomSpawner = RoomGenerator.Instance;

            foreach (var door in doors)
            {
                int rnd = Random.Range(0, roomSpawner.roomPrefabs.Length);
                Vector3 scale = new Vector3(
                    door.localPosition.x * roomSpawner.roomPrefabs[rnd].GetComponent<Room>().roomScale.x,
                    door.localPosition.y * roomSpawner.roomPrefabs[rnd].GetComponent<Room>().roomScale.y,
                    door.localPosition.z * roomSpawner.roomPrefabs[rnd].GetComponent<Room>().roomScale.z);

                // Get the rotation of the object this script is attached to
                Quaternion objectRotation = transform.rotation;
                // Calculate the rotation matrix from the object's rotation
                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(objectRotation);
                // Apply the rotation matrix to the scale vector
                scale = rotationMatrix.MultiplyVector(scale);

                scale *= 2; //multiply to go from doorPos to where the center needs to be of the new room
                Vector3 newRoomPos = transform.position + scale;
                //check if key is present in dictionary, meaning checking if a roompositions exists
                if (!roomSpawner.roomPositions.ContainsKey(newRoomPos))
                {
                    continue;
                }

                if (roomSpawner.roomPositions[newRoomPos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomSpawner.roomsLeftToSpawn > 0)
                {
                    roomSpawner.roomPositions[newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Yield;

                    //check surrounding room of next position
                    //foreach to check around the next room for doors
                    foreach (var possibledoorLocation in roomSpawner.room4Doors.GetComponent<Room>().doors)
                    {
                        //get status from all rooms around next room
                        if (roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().status == RoomStatus.Completed)
                        {
                            if (!roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().
                                roomInPosition)
                            {
                                Debug.Log("guhhhh", roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)]);
                                continue;
                            }
                            //check possible door position from surrounding rooms and check if that is an actual door
                            if (roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)].GetComponent<RoomPos>().
                                /* top part == if rooms around next possible spawn room
                                   bottom part == if door in surrounding rooms of next room is relevant */
                                roomInPosition.GetComponent<Room>().doorPositions.ContainsKey(newRoomPos + (possibledoorLocation.localPosition)))
                            {
                                Debug.Log(" hi", roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().
                                roomInPosition.GetComponent<Room>().doorPositions[newRoomPos + (possibledoorLocation.localPosition)]);
                                //this is not working correctly!!!!!!!
                                doorsAroundTheNextRoom.Add(newRoomPos + possibledoorLocation.localPosition);
                               // print(possibledoorLocation.localPosition);

                            }

                        }
                    }
                    int rndRot = 0;

                   // int rndRot = Random.Range(0, 4);
                    rndRot *= 90;

                    roomSpawner.roomsLeftToSpawn--;
                    GameObject spawnedRoom = Instantiate(roomSpawner.roomPrefabs[rnd], newRoomPos, Quaternion.Euler(0, rndRot, 0));
                    roomSpawner.roomPositions[newRoomPos].GetComponent<RoomPos>().roomInPosition = spawnedRoom;

                    RotateRoom(spawnedRoom, door, newRoomPos);
                }
                else if (roomSpawner.roomPositions[newRoomPos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomSpawner.roomsLeftToSpawn <= 0)
                {
                    roomSpawner.roomPositions[newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Yield;

                    GameObject spawnedRoom = Instantiate(roomSpawner.endRoom, newRoomPos, Quaternion.identity);
                    roomSpawner.roomPositions[newRoomPos].GetComponent<RoomPos>().roomInPosition = spawnedRoom;

                    RotateRoom(spawnedRoom, door, newRoomPos);
                }
            }
        }
        //TryReplace();
    }
    private void RotateRoom(GameObject spawnedRoom, Transform door, Vector3 pos)
    {
        roomSpawner.roomPositions[pos].GetComponent<RoomPos>().status = RoomStatus.Completed;

        spawnedRoom.GetComponent<Room>().Invoke(nameof(SpawnRooms), 0.5f);

        //for (int i = 0; i < 5; i++)
        //{
        //    if (spawnedRoom.GetComponent<Room>().doorPositions.ContainsKey(door.transform.position))
        //    {
        //        roomSpawner.roomPositions[pos].GetComponent<RoomPos>().status = RoomStatus.Completed;
        //        spawnedRoom.GetComponent<Room>().Invoke(nameof(SpawnRooms), 0.5f);
        //        break;
        //    }
        //    else
        //    {
        //        spawnedRoom.transform.Rotate(Vector3.up, 90);
        //        spawnedRoom.GetComponent<Room>().UpdateDictionary();
        //    }
        //    if (i == 4)
        //    {
        //        //roomSpawner.roomsLeftToSpawn++;
        //        //Destroy(gameObject);
        //       // Debug.Log("Couldnt correctly rotate", spawnedRoom);

        //    }
        //}
    }
    //[ContextMenu("update room")]
    //private void TryReplace()
    //{
    //    triedReplace = true;
    //    Collider[] checkDoorsColliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 1.9f, Quaternion.identity, LayerMask.GetMask("Door"));
    //    List<Collider> colliders = new();
    //    int doorsAroundTheNextRoom = checkDoorsColliders.Length - doors.Length;
    //    foreach (var coll in checkDoorsColliders)
    //    {
    //        if (coll.transform.parent.gameObject != gameObject)
    //        {
    //            colliders.Add(coll);
    //        }
    //    }
    //    doorsaround = doorsAroundTheNextRoom;

    //    if (doorsAroundTheNextRoom == 4 && doors.Length != 4)
    //    {
    //        Destroy(gameObject);
    //        GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room4Doors, transform.position, Quaternion.Euler(0, 0, 0));
    //        spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
    //        spawnedRoomWMoorDoors.GetComponent<Room>().replaced = true;

    //    }
    //    else if (doorsAroundTheNextRoom == 3 && doors.Length != 3)
    //    {
    //        GetComponent<BoxCollider>().enabled = false;
    //        foreach (Transform door in doors)
    //        {
    //            door.GetComponent<BoxCollider>().enabled = false;
    //        }
    //        GetComponent<MeshFilter>().sharedMesh = null;
    //        GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room3Doors, transform.position, Quaternion.Euler(0, 0, 0));
    //        spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
    //        spawnedRoomWMoorDoors.GetComponent<Room>().replaced = true;
    //        for (int i = 0; i < 20; i++)
    //        {
    //            bool allCollidersOnThisObjectAreTouchingDoor = true;
    //            foreach (var col in colliders)
    //            {
    //                if (col == null) continue;
    //                Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
    //                // Check if the collider is touching something tagged as "Door"
    //                if (overlapColliders.Length != 2) // Change this to the number of doors you expect to find
    //                {
    //                    allCollidersOnThisObjectAreTouchingDoor = false;
    //                }
    //            }
    //            if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
    //            {
    //                //no more rotations needed
    //                break;
    //            }
    //            else
    //            {
    //                spawnedRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
    //            }
    //        }
    //        Destroy(gameObject);
    //    }
    //    else if (doorsAroundTheNextRoom == 2 && doors.Length != 2)
    //    {
    //        //L room with 2 doors
    //        GetComponent<BoxCollider>().enabled = false;
    //        foreach (Transform door in doors)
    //        {
    //            door.GetComponent<BoxCollider>().enabled = false;
    //        }
    //        GetComponent<MeshFilter>().sharedMesh = null;
    //        GameObject spawnedLRoomWMoorDoors = Instantiate(roomSpawner.room2LDoors, transform.position, Quaternion.Euler(0, 0, 0));
    //        spawnedLRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
    //        spawnedLRoomWMoorDoors.GetComponent<Room>().replaced = true;

    //        bool correctlySpawnedLRoom = false;
    //        for (int i = 0; i < 20; i++)
    //        {
    //            bool allCollidersOnThisObjectAreTouchingDoor = true;
    //            foreach (var col in colliders)
    //            {
    //                if (col == null) continue;
    //                Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
    //                if (overlapColliders.Length != 2)
    //                {
    //                    allCollidersOnThisObjectAreTouchingDoor = false;
    //                }
    //            }
    //            if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
    //            {
    //                correctlySpawnedLRoom = true;
    //                //no more rotations needed
    //                break;
    //            }
    //            else
    //            {
    //                spawnedLRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
    //            }
    //        }
    //        Destroy(gameObject);

    //        //normal 2 door room
    //        if (!correctlySpawnedLRoom)
    //        {
    //            Destroy(spawnedLRoomWMoorDoors);
    //            GetComponent<BoxCollider>().enabled = false;
    //            foreach (Transform door in doors)
    //            {
    //                door.GetComponent<BoxCollider>().enabled = false;
    //            }
    //            GetComponent<MeshFilter>().sharedMesh = null;
    //            GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room2Doors, transform.position, Quaternion.Euler(0, 0, 0));
    //            spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
    //            spawnedRoomWMoorDoors.GetComponent<Room>().replaced = true;
    //            for (int i = 0; i < 20; i++)
    //            {
    //                bool allCollidersOnThisObjectAreTouchingDoor = true;
    //                foreach (var col in colliders)
    //                {
    //                    if (col == null) continue;
    //                    Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
    //                    // Check if the collider is touching something tagged as "Door"
    //                    if (overlapColliders.Length != 2)
    //                    {
    //                        allCollidersOnThisObjectAreTouchingDoor = false;
    //                    }
    //                }
    //                if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
    //                {
    //                    //no more rotations needed
    //                    break;
    //                }
    //                else
    //                {
    //                    spawnedRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
    //                }
    //            }
    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < 20; i++)
    //        {
    //            bool allCollidersOnThisObjectAreTouchingDoor = true;
    //            foreach (var col in colliders)
    //            {
    //                if (col == null) continue;
    //                Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
    //                if (overlapColliders.Length != 2)
    //                {
    //                    allCollidersOnThisObjectAreTouchingDoor = false;
    //                }
    //            }
    //            if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
    //            {
    //                //no more rotations needed
    //                break;
    //            }
    //            else
    //            {
    //                transform.Rotate(Vector3.up, 90);
    //            }
    //        }
    //    }
    //}
    //[ContextMenu("update doorsaround")]
    //public void RoomsAround()
    //{
    //    Collider[] checkDoorsColliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 1.9f, Quaternion.identity, LayerMask.GetMask("Door"));
    //    List<Collider> colliders = new();
    //    int doorsAroundTheNextRoom = checkDoorsColliders.Length - doors.Length;
    //    foreach (var coll in checkDoorsColliders)
    //    {
    //        if (coll.transform.parent.gameObject != gameObject)
    //        {
    //            colliders.Add(coll);
    //        }
    //    }
    //    doorsaround = doorsAroundTheNextRoom;
    //}
}
