using System.Collections.Generic;
using UnityEngine;
public class Room : MonoBehaviour
{
    public Transform[] doors;
    public Dictionary<Vector3, GameObject> doorPositions = new Dictionary<Vector3, GameObject>();
    public Transform[] notDoors;
    public Dictionary<Vector3, GameObject> notDoorPositions = new Dictionary<Vector3, GameObject>();

    public Transform[] oreSpawnPoints;
    public Transform[] enemySpawnPoints;

    public Vector3 roomScale = new Vector3(1, 1, 1);
    private RoomGenerator roomGenerator;

    public bool allowRoomSpawn = true;
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

        foreach (var notDoor in notDoors)
        {
            notDoorPositions.Add(notDoor.transform.position, notDoor.gameObject);
        }
    }
    public void UpdateDictionary()
    {
        doorPositions.Clear();
        foreach (var door in doors)
        {
            doorPositions.Add(door.transform.position, door.gameObject);
        }

        notDoorPositions.Clear();
        foreach (var notDoor in notDoors)
        {
            notDoorPositions.Add(notDoor.transform.position, notDoor.gameObject);
        }
    }
    //problem rn is that there is no delay making 1 giant snake
    //basicly because only 1 door in starter room will make spawns and only when that one is done it will try other doors
    //thats why the invoke is needed
    public void SpawnRooms()
    {
        if (allowRoomSpawn)
        {
            roomGenerator = RoomGenerator.Instance;

            foreach (var door in doors)
            {
                Vector3 scale = new Vector3(
                    door.localPosition.x * roomGenerator.room4Doors.GetComponent<Room>().roomScale.x,
                    door.localPosition.y * roomGenerator.room4Doors.GetComponent<Room>().roomScale.y,
                    door.localPosition.z * roomGenerator.room4Doors.GetComponent<Room>().roomScale.z);

                // Get the rotation of the object this script is attached to
                Quaternion objectRotation = transform.rotation;
                // Calculate the rotation matrix from the object's rotation
                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(objectRotation);
                // Apply the rotation matrix to the scale vector
                scale = rotationMatrix.MultiplyVector(scale);

                scale *= 2; //multiply to go from doorPos to where the center needs to be of the new room
                Vector3 newRoomPos = transform.position + scale;
                //check if key is present in dictionary, meaning checking if a roompositions exists
                if (!roomGenerator.roomPositions.ContainsKey(newRoomPos))
                {
                    continue;
                }

                Dictionary<Vector3, int> doorsAroundTheNextRoom = new Dictionary<Vector3, int>();
                Dictionary<Vector3, int> notDoorsAroundTheNextRoom = new Dictionary<Vector3, int>();
                Dictionary<Vector3, int> possibleDoorsAroundTheNextRoom = new Dictionary<Vector3, int>();

                if (roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomGenerator.roomsLeftToSpawn > 0)
                {
                    roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Yield;

                    //check surrounding room of next position
                    //foreach to check around the next room for doors
                    foreach (var possibledoorLocation in roomGenerator.room4Doors.GetComponent<Room>().doors)
                    {
                        //get status from all rooms around next room
                        if (roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().status == RoomStatus.Completed)
                        {
                            if (!roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().
                                roomInPosition)
                            {
                                Debug.LogError("guhhhh", roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)]);
                                continue;
                            }


                            //check possible door position from surrounding rooms and check if that is an actual door
                            if (roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)].GetComponent<RoomPos>().
                                /* top part == if rooms around next possible spawn room
                                   bottom part == if door in surrounding rooms of next room is relevant */
                                roomInPosition.GetComponent<Room>().doorPositions.ContainsKey(newRoomPos + possibledoorLocation.localPosition))
                            {
                                //Debug.Log(" hi", roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().
                                //roomInPosition.GetComponent<Room>().doorPositions[newRoomPos + (possibledoorLocation.localPosition)]);

                                //only add the local position and not newRoomPos + possibledoorLocation.position
                                doorsAroundTheNextRoom.Add(possibledoorLocation.localPosition, 1);//prettiest code ever

                            }
                            if (roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)].GetComponent<RoomPos>().
                                /* top part == if rooms around next possible spawn room
                                   bottom part == if notdoor in surrounding rooms of next room is relevant */
                                roomInPosition.GetComponent<Room>().notDoorPositions.ContainsKey(newRoomPos + possibledoorLocation.localPosition))
                            {

                                //Debug.Log(" hi", roomSpawner.roomPositions[newRoomPos + (possibledoorLocation.position * 2)].GetComponent<RoomPos>().
                                //roomInPosition.GetComponent<Room>().doorPositions[newRoomPos + (possibledoorLocation.localPosition)]);

                                //only add the local position and not newRoomPos + possibledoorLocation.position
                                notDoorsAroundTheNextRoom.Add(possibledoorLocation.localPosition, 1);//prettiest code ever

                            }
                        }
                    }

                    //make sure to add door possible door locations to list so that all lists combined amount to 4 doors total
                    foreach (var possibledoorLocation in roomGenerator.room4Doors.GetComponent<Room>().doors)
                    {
                        //if not doing local use + newposition in key too

                        if (!doorsAroundTheNextRoom.ContainsKey(possibledoorLocation.position) && !notDoorsAroundTheNextRoom.ContainsKey(possibledoorLocation.position))
                        {
                            //only add the local position and not newRoomPos + possibledoorLocation.position
                            possibleDoorsAroundTheNextRoom.Add(possibledoorLocation.position, 1);
                        }
                    }
                    //idea
                    //first make the door positions relative to the room so you get something like x:5 z:0
                    //possibly check if for opposing 2 door room by checking if 1 number is negative and then multiply it by -1
                    //doing this will get you 2 of the same numbers if the room isnt an 2 door L room but just a normal 2 door

                    float room1DoorAppearChance = 0.2f;
                    List<int> amountOfDoorsToSpawnRoomWith = new List<int>();
                    switch (possibleDoorsAroundTheNextRoom.Count + doorsAroundTheNextRoom.Count)
                    {
                        case 4://in this case its possible to spawn a 4 door room maximum
                            if (doorsAroundTheNextRoom.Count == 1)
                            {
                                //make it possible to spawn a room with 1-4 doors
                                if (Random.value < room1DoorAppearChance)
                                {
                                    amountOfDoorsToSpawnRoomWith.Add(1);
                                    amountOfDoorsToSpawnRoomWith.Add(2);
                                    amountOfDoorsToSpawnRoomWith.Add(3);
                                    amountOfDoorsToSpawnRoomWith.Add(4);
                                }
                                else
                                {
                                    amountOfDoorsToSpawnRoomWith.Add(2);
                                    amountOfDoorsToSpawnRoomWith.Add(3);
                                    amountOfDoorsToSpawnRoomWith.Add(4);
                                }

                            }
                            else if (doorsAroundTheNextRoom.Count == 2)
                            {
                                //make it possible to spawn a room with 2-4 doors
                                amountOfDoorsToSpawnRoomWith.Add(2);
                                amountOfDoorsToSpawnRoomWith.Add(3);
                                amountOfDoorsToSpawnRoomWith.Add(4);
                            }
                            else if (doorsAroundTheNextRoom.Count == 3)
                            {
                                //make it possible to spawn a room with 3-4 doors
                                amountOfDoorsToSpawnRoomWith.Add(3);
                                amountOfDoorsToSpawnRoomWith.Add(4);

                            }
                            else if (doorsAroundTheNextRoom.Count == 4)
                            {
                                //make it possible to spawn a room with 4-4 doors
                                amountOfDoorsToSpawnRoomWith.Add(4);
                            }

                            break;

                        case 3://in this case its possible to spawn a 3 door room maximum
                            if (doorsAroundTheNextRoom.Count == 1)
                            {
                                //make it possible to spawn a room with 1-3 doors
                                if (Random.value < room1DoorAppearChance)
                                {
                                    amountOfDoorsToSpawnRoomWith.Add(1);
                                    amountOfDoorsToSpawnRoomWith.Add(2);
                                    amountOfDoorsToSpawnRoomWith.Add(3);
                                }
                                else
                                {
                                    amountOfDoorsToSpawnRoomWith.Add(2);
                                    amountOfDoorsToSpawnRoomWith.Add(3);
                                }

                            }
                            else if (doorsAroundTheNextRoom.Count == 2)
                            {
                                //make it possible to spawn a room with 2-3 doors

                                amountOfDoorsToSpawnRoomWith.Add(2);
                                amountOfDoorsToSpawnRoomWith.Add(3);

                            }
                            else if (doorsAroundTheNextRoom.Count == 3)
                            {
                                //make it possible to spawn a room with 3-3 doors

                                amountOfDoorsToSpawnRoomWith.Add(3);
                            }
                            break;

                        case 2://in this case its possible to spawn a 2 door room maximum
                            if (doorsAroundTheNextRoom.Count == 1)
                            {
                                //make it possible to spawn a room with 1-2 doors
                                if (Random.value < room1DoorAppearChance)
                                {
                                    amountOfDoorsToSpawnRoomWith.Add(1);
                                    amountOfDoorsToSpawnRoomWith.Add(2);

                                }
                                else//spawn room with 2 doors to not end the spawning here with a 1 door room
                                {
                                    amountOfDoorsToSpawnRoomWith.Add(2);
                                }
                            }
                            else if (doorsAroundTheNextRoom.Count == 2)
                            {
                                //make it possible to spawn a room with 2-2 doors

                                amountOfDoorsToSpawnRoomWith.Add(2);
                            }

                            break;

                        case 1://in this case its only possible to spawn a 1 door room
                            //make it possible to spawn a room with 1 door
                            amountOfDoorsToSpawnRoomWith.Add(1);
                            break;

                        default:
                            print("all rooms failed spawning");
                            break;
                    }
                    int rnd = Random.Range(0, amountOfDoorsToSpawnRoomWith.Count);
                    if (amountOfDoorsToSpawnRoomWith[rnd] == 4)
                    {
                        //no rotation needed
                        SpawnRoom(0, newRoomPos, roomGenerator.room4Doors);
                    }
                    else if (amountOfDoorsToSpawnRoomWith[rnd] == 3)
                    {
                        //need to add rotation
                        int rotationsNeeded = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            foreach (var doorInroom in roomGenerator.room3Doors.GetComponent<Room>().doors)
                            {
                                if (roomGenerator.printNeeded)
                                {
                                    Debug.Log(doorInroom.position, roomGenerator.roomPositions[newRoomPos]);

                                }
                                //if required door position isnt matching positions with room thats gonna be spawned
                                //make sure to have a door at a needed door position and also make sure to not have a door and a non door position
                                if (doorsAroundTheNextRoom.ContainsKey(doorInroom.position) && !notDoorsAroundTheNextRoom.ContainsKey(doorInroom.position))
                                {
                                    if (roomGenerator.printNeeded)
                                    {
                                        print("hi");
                                    }
                                    
                                    //no need for rotation
                                }
                                else
                                {
                                    Vector3 rotateAround = new Vector3(0, 0, 0);
                                    rotationsNeeded++;
                                    //rotate here
                                    foreach (Transform position in roomGenerator.room3Doors.GetComponent<Room>().doors)
                                    {
                                        position.Rotate(rotateAround, 90);
                                    }
                                    foreach (Transform position in roomGenerator.room3Doors.GetComponent<Room>().notDoors)
                                    {
                                        position.Rotate(rotateAround, 90);
                                    }
                                }
                            }
                        }
                        foreach (var doorInroom in roomGenerator.room3Doors.GetComponent<Room>().doors)
                        {
                            print(doorInroom.position);
                        }
                        roomGenerator.printNeeded = false;




                        SpawnRoom(rotationsNeeded * 90, newRoomPos, roomGenerator.room3Doors);
                    }
                    else if (amountOfDoorsToSpawnRoomWith[rnd] == 2)
                    {
                        //need to add rotation
                        SpawnRoom(0, newRoomPos, roomGenerator.room2Doors);
                    }
                    else if (amountOfDoorsToSpawnRoomWith[rnd] == 1)
                    {
                        //need to add rotation
                        SpawnRoom(0, newRoomPos, roomGenerator.endRoom);
                    }

                    roomGenerator.roomsLeftToSpawn--;
                    // RotateRoom(spawnedRoom, door, newRoomPos);

                }
                else if (roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomGenerator.roomsLeftToSpawn <= 0)
                {
                    int rotation = 0;
                    roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Yield;
                    SpawnRoom(rotation, newRoomPos, roomGenerator.endRoom);
                    GameObject spawnedRoom = Instantiate(roomGenerator.endRoom, newRoomPos, Quaternion.identity);

                }
            }
        }
        //TryReplace();
    }
    private void SpawnRoom(int _rotation, Vector3 _newRoomPos, GameObject roomToSpawn)
    {
        GameObject spawnedRoom = Instantiate(roomToSpawn, _newRoomPos, Quaternion.Euler(0, _rotation, 0));
        roomGenerator.roomPositions[_newRoomPos].GetComponent<RoomPos>().roomInPosition = spawnedRoom;
        roomGenerator.roomPositions[_newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Completed;
        spawnedRoom.GetComponent<Room>().Invoke(nameof(SpawnRooms), 0.5f);


    }
    //private void RotateRoom(GameObject spawnedRoom, Transform door, Vector3 pos)
    //{
    //    roomGenerator.roomPositions[pos].GetComponent<RoomPos>().status = RoomStatus.Completed;

    //    spawnedRoom.GetComponent<Room>().Invoke(nameof(SpawnRooms), 0.5f);

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
    //}
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
