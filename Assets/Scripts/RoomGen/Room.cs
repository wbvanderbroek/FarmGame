using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] doors;
    public Dictionary<Vector3, GameObject> doorPositions = new Dictionary<Vector3, GameObject>();
    public Transform[] notDoors;
    public Dictionary<Vector3, GameObject> notDoorPositions = new Dictionary<Vector3, GameObject>();

    public Transform[] oreSpawnPoints;
    public Transform[] enemySpawnPoints;

    //for now just keep it at 1
    public Vector3 roomScale = new Vector3(1, 1, 1);
    private RoomGenerator roomGenerator;

    public bool allowRoomSpawn = true;
    public float spawnChance = 0.5f;
    private void SpawnOres()
    {
        foreach (Transform spawnPoint in oreSpawnPoints)
        {
            if (Random.value < spawnChance)
            {
                int rnd = Random.Range(0, roomGenerator.orePool.Length);
                GameObject orePrefab = roomGenerator.orePool[rnd];
                Instantiate(orePrefab, spawnPoint.position, Quaternion.identity);
            }
        }
        roomGenerator.OnDone -= SpawnOres;

    }
    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            if (Random.value < spawnChance)
            {
                int rnd = Random.Range(0, roomGenerator.enemyPool.Length);
                GameObject orePrefab = roomGenerator.enemyPool[rnd];
                Instantiate(orePrefab, spawnPoint.position, Quaternion.identity);
            }
        }
        roomGenerator.OnDone -= SpawnEnemies;

    }
    private void Awake()
    {
        roomGenerator = RoomGenerator.Instance;
        roomGenerator.OnDone += SpawnOres;
        roomGenerator.OnDone += SpawnEnemies;

        UpdateDictionary();
    }
    public void UpdateDictionary()
    {
        doorPositions.Clear();
        foreach (var door in doors)
        {
            doorPositions.Add(door.transform.position, door.gameObject);
            roomGenerator.allDoorPositions.Add(roomGenerator.allDoorPositions.Count + 1, door.position);
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

            foreach (var door in doors)
            {
                Vector3 scale = new Vector3(
                    door.localPosition.x * roomGenerator.room4Doors.GetComponent<Room>().roomScale.x,
                    door.localPosition.y * roomGenerator.room4Doors.GetComponent<Room>().roomScale.y,
                    door.localPosition.z * roomGenerator.room4Doors.GetComponent<Room>().roomScale.z);

                // Get the rotation of the object this script is attached to
                Quaternion objectRotation = transform.rotation;
                // Rotate the scale vector according to the rotation of the main object
                Vector3 scaledRotatedOffset = objectRotation * scale;

                scaledRotatedOffset *= 2; // multiply to go from doorPos to where the center needs to be of the new room
                Vector3 newRoomPos = transform.position + scaledRotatedOffset;
                //need to round because unity is super duper stupid and will make this something like 9.9999
                newRoomPos.x = Mathf.Round(newRoomPos.x);
                newRoomPos.y = Mathf.Round(newRoomPos.y);
                newRoomPos.z = Mathf.Round(newRoomPos.z);

                //check if key is present in dictionary, meaning checking if a roompositions exists
                if (!roomGenerator.roomPositions.ContainsKey(newRoomPos))
                {
                    Debug.LogWarning("couldnt find the key in roompositions dictionary, THIS SHOULD NEVER HAPPEN EXCEPT WHEN AT THE EDGE OF THE GRID");
                    // print(newRoomPos);
                    continue;
                }

                Dictionary<Vector3, int> doorsAroundTheNextRoom = new Dictionary<Vector3, int>();
                Dictionary<Vector3, int> notDoorsAroundTheNextRoom = new Dictionary<Vector3, int>();
                Dictionary<Vector3, int> possibleDoorsAroundTheNextRoom = new Dictionary<Vector3, int>();
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
                            Debug.LogError("this should not happen!!", roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)]);
                            continue;
                        }


                        //check possible door position from surrounding rooms and check if that is an actual door
                        if (roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)].GetComponent<RoomPos>().
                            /* top part == if rooms around next possible spawn room
                               bottom part == if door in surrounding rooms of next room is relevant */
                            roomInPosition.GetComponent<Room>().doorPositions.ContainsKey(newRoomPos + possibledoorLocation.localPosition))
                        {
                            //only add the local position and not newRoomPos + possibledoorLocation.position
                            doorsAroundTheNextRoom.Add(possibledoorLocation.localPosition + newRoomPos, 1);//prettiest code ever

                        }
                        if (roomGenerator.roomPositions[newRoomPos + (possibledoorLocation.localPosition * 2)].GetComponent<RoomPos>().
                            /* top part == if rooms around next possible spawn room
                               bottom part == if notdoor in surrounding rooms of next room is relevant */
                            roomInPosition.GetComponent<Room>().notDoorPositions.ContainsKey(newRoomPos + possibledoorLocation.localPosition))
                        {
                            //only add the local position and not newRoomPos + possibledoorLocation.position
                            notDoorsAroundTheNextRoom.Add(possibledoorLocation.localPosition + newRoomPos, 1);//prettiest code ever

                        }
                    }
                }
                //make sure to add door possible door locations to list so that all lists combined amount to 4 doors total
                foreach (var possibledoorLocation in roomGenerator.room4Doors.GetComponent<Room>().doors)
                {
                    //if not doing local use + newposition in key too

                    if (!doorsAroundTheNextRoom.ContainsKey(possibledoorLocation.position + newRoomPos) && !notDoorsAroundTheNextRoom.ContainsKey(possibledoorLocation.position + newRoomPos))
                    {
                        //only add the local position and not newRoomPos + possibledoorLocation.position
                        possibleDoorsAroundTheNextRoom.Add(possibledoorLocation.localPosition + newRoomPos, 1);
                    }
                }
                if (roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomGenerator.roomsLeftToSpawn > 0)
                {
                    roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Yield;


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
                                    //amountOfDoorsToSpawnRoomWith.Add(1);
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
                            else
                            {
                                //this happens because the doors are rotated but they use the unrotated doors to spawn more rooms
                                amountOfDoorsToSpawnRoomWith.Add(2);

                                //print("something went wrong in case 4: doorsaroundnextroom is " + doorsAroundTheNextRoom.Count +
                                //" and possible locationcount is: " + possibleDoorsAroundTheNextRoom.Count);
                            }
                            break;

                        case 3://in this case its possible to spawn a 3 door room maximum
                            if (doorsAroundTheNextRoom.Count == 1)
                            {
                                //make it possible to spawn a room with 1-3 doors
                                if (Random.value < room1DoorAppearChance)
                                {
                                    //amountOfDoorsToSpawnRoomWith.Add(1);
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
                            else
                            {
                                amountOfDoorsToSpawnRoomWith.Add(3);

                                //print("something went wrong in case 3: " + doorsAroundTheNextRoom.Count);
                            }
                            break;

                        case 2://in this case its possible to spawn a 2 door room maximum
                            if (doorsAroundTheNextRoom.Count == 1)
                            {
                                //make it possible to spawn a room with 1-2 doors
                                if (Random.value < room1DoorAppearChance)
                                {
                                    //amountOfDoorsToSpawnRoomWith.Add(1);
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
                            else
                            {
                                amountOfDoorsToSpawnRoomWith.Add(2);
                                //print("something went wrong in case 2: " + doorsAroundTheNextRoom.Count);
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
                    //print(amountOfDoorsToSpawnRoomWith[rnd]);
                    bool stillNeeded = true;
                    switch (amountOfDoorsToSpawnRoomWith[rnd])
                    {
                        case 4:
                            //done
                            SpawnRoom(0, newRoomPos, roomGenerator.room4Doors);
                            break;
                        case 3:
                            // done?
                            if (stillNeeded)
                            {
                                if (CorrectRoomRotationFinder3Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room3Doors.GetComponent<Room>().doors))
                                {
                                    SpawnRoom(0, newRoomPos, roomGenerator.room3Doors);
                                    stillNeeded = false;
                                }
                            }
                            if (stillNeeded)
                            {
                                if (CorrectRoomRotationFinder3Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room3Doors90.GetComponent<Room>().doors))
                                {
                                    SpawnRoom(90, newRoomPos, roomGenerator.room3Doors);
                                    stillNeeded = false;
                                }
                            }
                            if (stillNeeded)
                            {
                                if (CorrectRoomRotationFinder3Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room3Doors180.GetComponent<Room>().doors))
                                {
                                    SpawnRoom(180, newRoomPos, roomGenerator.room3Doors);
                                    stillNeeded = false;
                                }
                            }
                            if (stillNeeded)
                            {
                                if (CorrectRoomRotationFinder3Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room3Doors270.GetComponent<Room>().doors))
                                {
                                    SpawnRoom(270, newRoomPos, roomGenerator.room3Doors);
                                    stillNeeded = false;
                                }
                            }
                            if (stillNeeded)
                            {
                                //Debug.Log("room with 3 doors failed spawning " + doorsAroundTheNextRoom.Count + "   " + notDoorsAroundTheNextRoom.Count + " " + gameObject.name, gameObject);
                            }
                            break;
                        case 2:
                            //idea
                            //first make the door positions relative to the room so you get something like x:5 z:0
                            //possibly check if for opposing 2 door room by checking if 1 number is negative and then multiply it by -1
                            //doing this will get you 2 of the same numbers if the room isnt an 2 door L room but just a normal 2 door

                            foreach (var item in doorsAroundTheNextRoom.Keys)
                            {
                                if (item != (door.position + newRoomPos) && stillNeeded)
                                {
                                    Vector3 tempVec = item - newRoomPos;
                                    Vector3 scaleVec = new Vector3(-1, -1, -1);
                                    tempVec.Scale(scaleVec);
                                    if ((tempVec + newRoomPos) == door.position)
                                    {
                                        //doors at opposite ends of room
                                        if (stillNeeded)
                                        {
                                            foreach (Transform doorInroom in roomGenerator.room2Doors.GetComponent<Room>().doors)
                                            {
                                                if (doorInroom.position + newRoomPos == door.position)
                                                {
                                                    SpawnRoom(0, newRoomPos, roomGenerator.room2Doors);
                                                    stillNeeded = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (stillNeeded)
                                        {
                                            foreach (Transform doorInroom in roomGenerator.room2Doors90.GetComponent<Room>().doors)
                                            {
                                                if (doorInroom.position + newRoomPos == door.position)
                                                {
                                                    SpawnRoom(90, newRoomPos, roomGenerator.room2Doors);
                                                    stillNeeded = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (stillNeeded)
                                        {
                                            //Debug.Log("room with 2 doors at each end of room failed spawning" + doorsAroundTheNextRoom.Count + "   " + notDoorsAroundTheNextRoom.Count + "    " + gameObject.name, gameObject);
                                        }
                                    }
                                    else
                                    {
                                        //L room
                                        if (stillNeeded)
                                        {
                                            if (CorrectRoomRotationFinder2Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room2LDoors.GetComponent<Room>().doors))
                                            {
                                                SpawnRoom(0, newRoomPos, roomGenerator.room2LDoors);
                                                stillNeeded = false;
                                            }
                                        }
                                        if (stillNeeded)
                                        {
                                            if (CorrectRoomRotationFinder2Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room2LDoors90.GetComponent<Room>().doors))
                                            {
                                                SpawnRoom(90, newRoomPos, roomGenerator.room2LDoors);
                                                stillNeeded = false;
                                            }
                                        }
                                        if (stillNeeded)
                                        {
                                            if (CorrectRoomRotationFinder2Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room2LDoors180.GetComponent<Room>().doors))
                                            {
                                                SpawnRoom(180, newRoomPos, roomGenerator.room2LDoors);
                                                stillNeeded = false;
                                            }
                                        }
                                        if (stillNeeded)
                                        {
                                            if (CorrectRoomRotationFinder2Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.room2LDoors270.GetComponent<Room>().doors))
                                            {
                                                SpawnRoom(270, newRoomPos, roomGenerator.room2LDoors);
                                                stillNeeded = false;
                                            }
                                        }
                                        if (stillNeeded)
                                        {
                                            //Debug.Log("2 door L room failed spawning  " + doorsAroundTheNextRoom.Count + "   " + notDoorsAroundTheNextRoom.Count + "    " + gameObject.name, gameObject);
                                        }
                                    }
                                }   
                            }
                            // temp code solution!!!!!!!!!!!!
                            if (stillNeeded)
                            {
                                TrySpawn1DoorRoom(door, newRoomPos, notDoorsAroundTheNextRoom);
                            }
                            break;
                        case 1:
                            //done
                            TrySpawn1DoorRoom(door, newRoomPos, notDoorsAroundTheNextRoom);
                            break;
                        default:
                            print("Out of range!");
                            break;

                    }
                }
                else if (roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomGenerator.roomsLeftToSpawn <= 0)
                {
                    //might need some work, possibly spawn room with doorsAroundTheNextRoom.Count
                    roomGenerator.roomPositions[newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Yield;
                    TrySpawn1DoorRoom(door, newRoomPos, notDoorsAroundTheNextRoom);
                }
                else
                {
                    int doorsatposiition = 0;
                    foreach (var value in roomGenerator.allDoorPositions.Values)
                    {
                        if (value == door.position)
                        {
                            doorsatposiition++;
                        }
                    }
                    if (doorsatposiition == 1)
                    {
                        GameObject doorCloser = Instantiate(roomGenerator.doorCloser, door.position, Quaternion.identity, transform);
                        doorCloser.transform.LookAt(transform.position);
                        doorCloser.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        doorCloser.transform.localPosition = new Vector3(doorCloser.transform.localPosition.x * 0.95f, doorCloser.transform.localPosition.y - 6, doorCloser.transform.localPosition.z * 0.95f);
                    }
                }
            }
        }
    }
    private void TrySpawn1DoorRoom(Transform door, Vector3 newRoomPos, Dictionary<Vector3, int> notDoorsAroundTheNextRoom)
    {
        bool stillNeeded = true;
        if (stillNeeded)
        {
            if (CorrectRoomRotationFinder1Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.endRoom.GetComponent<Room>().doors))
            {
                SpawnRoom(0, newRoomPos, roomGenerator.endRoom);
                stillNeeded = false;
            }
        }
        if (stillNeeded)
        {
            if (CorrectRoomRotationFinder1Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.endRoom90.GetComponent<Room>().doors))
            {
                SpawnRoom(90, newRoomPos, roomGenerator.endRoom);
                stillNeeded = false;
            }
        }
        if (stillNeeded)
        {
            if (CorrectRoomRotationFinder1Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.endRoom180.GetComponent<Room>().doors))
            {
                SpawnRoom(180, newRoomPos, roomGenerator.endRoom);
                stillNeeded = false;
            }
        }
        if (stillNeeded)
        {
            if (CorrectRoomRotationFinder1Door(door, newRoomPos, notDoorsAroundTheNextRoom, roomGenerator.endRoom270.GetComponent<Room>().doors))
            {
                SpawnRoom(270, newRoomPos, roomGenerator.endRoom);
                stillNeeded = false;
            }
        }
        if (stillNeeded)
        {
            Debug.Log("room wiht 1 door failed spawning", gameObject);
        }
    }
    private bool CorrectRoomRotationFinder1Door(Transform door, Vector3 newRoomPos, Dictionary<Vector3, int> notDoorsAroundTheNextRoom, Transform[] prefabDoors)
    {

        foreach (Transform doorInroom in prefabDoors)
        {
            if (doorInroom.position + newRoomPos == door.position)
            {
                return true;
            }
        }
        return false;
    }

    private bool CorrectRoomRotationFinder2Door(Transform door, Vector3 newRoomPos, Dictionary<Vector3, int> notDoorsAroundTheNextRoom, Transform[] prefabDoors)
    {
        bool _doorAtDoorLocation = false;
        int integer = 2;
        foreach (Transform doorInroom in prefabDoors)
        {
            if (((doorInroom.position + newRoomPos) == door.position) || _doorAtDoorLocation == true)
            {
                _doorAtDoorLocation = true;
                if (!notDoorsAroundTheNextRoom.ContainsKey(doorInroom.position + newRoomPos))
                {
                    integer--;
                }
                else if (notDoorsAroundTheNextRoom.Count == 0)
                {
                    integer--;
                }
            }
        }
        if (integer <= 0)
        {
            return true;
        }
        return false;
    }
    private bool CorrectRoomRotationFinder3Door(Transform door, Vector3 newRoomPos, Dictionary<Vector3, int> notDoorsAroundTheNextRoom, Transform[] prefabDoors)
    {
        bool _doorAtDoorLocation = false;
        foreach (Transform doorInroom in prefabDoors)
        {
            if (((doorInroom.position + newRoomPos) == door.position) || _doorAtDoorLocation == true)
            {
                _doorAtDoorLocation = true;
                if (!notDoorsAroundTheNextRoom.ContainsKey(doorInroom.position + newRoomPos))
                {
                    return true;
                }
                else if (notDoorsAroundTheNextRoom.Count == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void SpawnRoom(int _rotation, Vector3 _newRoomPos, GameObject roomToSpawn)
    {
        roomGenerator.roomsLeftToSpawn--;

        GameObject spawnedRoom = Instantiate(roomToSpawn, _newRoomPos, Quaternion.Euler(0, _rotation, 0));
        roomGenerator.roomPositions[_newRoomPos].GetComponent<RoomPos>().roomInPosition = spawnedRoom;
        roomGenerator.roomPositions[_newRoomPos].GetComponent<RoomPos>().status = RoomStatus.Completed;
        spawnedRoom.GetComponent<Room>().Invoke(nameof(SpawnRooms), 0.2f);

    }
}
