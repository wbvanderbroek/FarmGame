using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] doors;
    public Transform[] notDoors;
    public Transform[] oreSpawnPoints;
    public Transform[] enemySpawnPoints;

    public Vector3 roomScale = new Vector3(1, 1, 1);
    private RoomGenerator roomSpawner;

    public bool AllowRoomSpawn = true;
    public bool triedReplace;
    public bool replaced;
    public int doorsaround;
    public float spawnChance = 0.5f;


    private void Start()
    {
        SpawnRooms();
    }

    public void SpawnRooms()
    {
        if (AllowRoomSpawn)
        {
            roomSpawner = RoomGenerator.Instance;

            foreach (var door in doors)
            {
                if(roomSpawner == null)
                {
                    Debug.LogWarning("Object no existo registrato!");
                }

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

                scale *= 2; //Multiply to go from doorPos to where the center needs to be of the new room
                Vector3 pos = transform.position + scale;
                
                if (roomSpawner.roomPositions[pos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomSpawner.roomsLeftToSpawn > 0)
                {
                    roomSpawner.roomsLeftToSpawn--;
                    roomSpawner.roomPositions[pos].GetComponent<RoomPos>().status = RoomStatus.Completed;
                    GameObject spawnedRoom = Instantiate(roomSpawner.roomPrefabs[rnd], pos, Quaternion.identity);
                }
                else if (roomSpawner.roomPositions[pos].GetComponent<RoomPos>().status == RoomStatus.Empty && roomSpawner.roomsLeftToSpawn <= 0)
                {
                    roomSpawner.roomPositions[pos].GetComponent<RoomPos>().status = RoomStatus.Completed;
                    GameObject spawnedRoom = Instantiate(roomSpawner.endRoom, pos, Quaternion.identity);
                }
                
            }
        }
        //TryReplace();

    }
    //possibly remove transforms when 2 are at the same position and use this to check for collision

    [ContextMenu("update room")]
    private void TryReplace()
    {
        triedReplace = true;
        Collider[] checkDoorsColliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 1.9f, Quaternion.identity, LayerMask.GetMask("Door"));
        List<Collider> colliders = new();
        int doorsAroundRoom = checkDoorsColliders.Length - doors.Length;
        foreach (var coll in checkDoorsColliders)
        {
            if (coll.transform.parent.gameObject != gameObject)
            {
                colliders.Add(coll);
            }
        }
        doorsaround = doorsAroundRoom;

        if (doorsAroundRoom == 4 && doors.Length != 4)
        {
            Destroy(gameObject);
            GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room4Doors, transform.position, Quaternion.Euler(0, 0, 0));
            spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
            spawnedRoomWMoorDoors.GetComponent<Room>().replaced = true;

        }
        else if (doorsAroundRoom == 3 && doors.Length != 3)
        {
            GetComponent<BoxCollider>().enabled = false;
            foreach (Transform door in doors)
            {
                door.GetComponent<BoxCollider>().enabled = false;
            }
            GetComponent<MeshFilter>().sharedMesh = null;
            GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room3Doors, transform.position, Quaternion.Euler(0, 0, 0));
            spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
            spawnedRoomWMoorDoors.GetComponent<Room>().replaced = true;
            for (int i = 0; i < 20; i++)
            {
                bool allCollidersOnThisObjectAreTouchingDoor = true;
                foreach (var col in colliders)
                {
                    if (col == null) continue;
                    Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
                    // Check if the collider is touching something tagged as "Door"
                    if (overlapColliders.Length != 2) // Change this to the number of doors you expect to find
                    {
                        allCollidersOnThisObjectAreTouchingDoor = false;
                    }
                }
                if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
                {
                    //no more rotations needed
                    break;
                }
                else
                {
                    spawnedRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
                }
            }
            Destroy(gameObject);
        }
        else if (doorsAroundRoom == 2 && doors.Length != 2)
        {
            //L room with 2 doors
            GetComponent<BoxCollider>().enabled = false;
            foreach (Transform door in doors)
            {
                door.GetComponent<BoxCollider>().enabled = false;
            }
            GetComponent<MeshFilter>().sharedMesh = null;
            GameObject spawnedLRoomWMoorDoors = Instantiate(roomSpawner.room2LDoors, transform.position, Quaternion.Euler(0, 0, 0));
            spawnedLRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
            spawnedLRoomWMoorDoors.GetComponent<Room>().replaced = true;

            bool correctlySpawnedLRoom = false;
            for (int i = 0; i < 20; i++)
            {
                bool allCollidersOnThisObjectAreTouchingDoor = true;
                foreach (var col in colliders)
                {
                    if (col == null) continue;
                    Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
                    if (overlapColliders.Length != 2)
                    {
                        allCollidersOnThisObjectAreTouchingDoor = false;
                    }
                }
                if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
                {
                    correctlySpawnedLRoom = true;
                    //no more rotations needed
                    break;
                }
                else
                {
                    spawnedLRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
                }
            }
            Destroy(gameObject);

            //normal 2 door room
            if (!correctlySpawnedLRoom)
            {
                Destroy(spawnedLRoomWMoorDoors);
                GetComponent<BoxCollider>().enabled = false;
                foreach (Transform door in doors)
                {
                    door.GetComponent<BoxCollider>().enabled = false;
                }
                GetComponent<MeshFilter>().sharedMesh = null;
                GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room2Doors, transform.position, Quaternion.Euler(0, 0, 0));
                spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
                spawnedRoomWMoorDoors.GetComponent<Room>().replaced = true;
                for (int i = 0; i < 20; i++)
                {
                    bool allCollidersOnThisObjectAreTouchingDoor = true;
                    foreach (var col in colliders)
                    {
                        if (col == null) continue;
                        Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
                        // Check if the collider is touching something tagged as "Door"
                        if (overlapColliders.Length != 2)
                        {
                            allCollidersOnThisObjectAreTouchingDoor = false;
                        }
                    }
                    if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
                    {
                        //no more rotations needed
                        break;
                    }
                    else
                    {
                        spawnedRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
                    }
                }
                Destroy(gameObject);
            }
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                bool allCollidersOnThisObjectAreTouchingDoor = true;
                foreach (var col in colliders)
                {
                    if (col == null) continue;
                    Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
                    if (overlapColliders.Length != 2)
                    {
                        allCollidersOnThisObjectAreTouchingDoor = false;
                    }
                }
                if (allCollidersOnThisObjectAreTouchingDoor && colliders.Count > 0)
                {
                    //no more rotations needed
                    break;
                }
                else
                {
                    transform.Rotate(Vector3.up, 90);
                }
            }
        }
    }
    [ContextMenu("update doorsaround")]
    public void RoomsAround()
    {
        Collider[] checkDoorsColliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 1.9f, Quaternion.identity, LayerMask.GetMask("Door"));
        List<Collider> colliders = new();
        int doorsAroundRoom = checkDoorsColliders.Length - doors.Length;
        foreach (var coll in checkDoorsColliders)
        {
            if (coll.transform.parent.gameObject != gameObject)
            {
                colliders.Add(coll);
            }
        }
        doorsaround = doorsAroundRoom;
    }
}
