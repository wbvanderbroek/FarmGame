using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] doors;
    public Vector3 roomScale = new Vector3(1, 1, 1);
    private RoomSpawner roomSpawner;
    public bool AllowRoomSpawn = true;

    private void Start()
    {
        roomSpawner = RoomSpawner.Instance;
    }
    public IEnumerator SpawnRooms()
    {
        yield return new WaitForSeconds(0.01f);
        if (AllowRoomSpawn)
        {
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

                scale *= 2; //Multiply to go from doorPos to where the center needs to be of the new room
                Vector3 pos = transform.position + scale;
                Collider[] colliders = Physics.OverlapBox(pos, roomSpawner.roomPrefabs[rnd].GetComponent<BoxCollider>().size / 2.01f, Quaternion.identity, ~(1 << LayerMask.NameToLayer("Door")));

                //Gizmos
                //roomPos.Add(pos);
                roomSize = roomSpawner.roomPrefabs[rnd].GetComponent<BoxCollider>().size / 1.005f;
                //End gizmos

                if (colliders.Length == 0)
                {

                    if (roomSpawner.roomsLeftToSpawn > 0)
                    {
                        roomSpawner.roomsLeftToSpawn--;
                        int rndRot = Random.Range(0, 4);
                        rndRot *= 90;
                        GameObject spawnedRoom = Instantiate(roomSpawner.roomPrefabs[rnd], pos, Quaternion.Euler(0, rndRot, 0));

                        for (int i = 0; i < 10; i++)
                        {
                            Collider[] overlapColliders = Physics.OverlapSphere(door.position, 1f, LayerMask.GetMask("Door"));

                            doorPos.Add(door.position);
                            List<GameObject> collList = new List<GameObject>();
                            if (overlapColliders.Length > 0)
                            {
                                foreach (Collider collider in overlapColliders)
                                {
                                    collList.Add(collider.transform.parent.gameObject);
                                }
                            }

                            if (collList.Contains(gameObject) && collList.Contains(spawnedRoom))
                            {
                                if (overlapColliders[0].transform.position == overlapColliders[1].transform.position)
                                {
                                    //exit the loop because door positions are matching
                                    break;
                                }
                                else
                                {
                                    spawnedRoom.transform.Rotate(Vector3.up, 90);
                                }

                            }
                            else
                            {
                                spawnedRoom.transform.Rotate(Vector3.up, 90);
                            }
                        }
                        StartCoroutine(spawnedRoom.GetComponent<Room>().SpawnRooms());

                    }
                    else if (roomSpawner.roomsLeftToSpawn == 0)
                    {
                        GameObject finalRoom = Instantiate(roomSpawner.endRoom, pos, Quaternion.identity);
                        bool doorFound = false;
                        while (!doorFound)
                        {
                            foreach (var door2 in finalRoom.GetComponent<Room>().doors)
                            {
                                if (door.position == door2.position)
                                {
                                    doorFound = true;

                                    break;
                                }
                                finalRoom.transform.Rotate(Vector3.up, 90);

                            }
                            yield return new WaitForSeconds(1f);

                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(TryReplace());
    }
    private IEnumerator TryReplace()
    {
        Collider[] checkDoorsColliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2.01f, Quaternion.identity, LayerMask.GetMask("Door"));
        List<Collider> colliders = new();
        int doorsAroundRoom = checkDoorsColliders.Length - doors.Length;
        foreach (var coll in checkDoorsColliders)
        {
            if (coll.transform.parent.gameObject != gameObject)
            {
                colliders.Add(coll);
            }
        }
        if (doorsAroundRoom == 3 && doors.Length != 3)
        {
            StopCoroutine(SpawnRooms());
            Destroy(gameObject);
            GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room3Doors, transform.position, Quaternion.Euler(0, 0, 0));
            spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
            while (true)
            {
                print(colliders.Count);

                bool allCollidersOnThisObjectAreTouchingDoor = true;

                foreach (var col in colliders)
                {
                    // Check if the collider is touching something tagged as "Door"
                    Collider[] overlapColliders = Physics.OverlapSphere(col.transform.position, 1f, LayerMask.GetMask("Door"));
                    if (overlapColliders.Length != 2) // Change this to the number of doors you expect to find
                    {
                        allCollidersOnThisObjectAreTouchingDoor = false;
                    }
                }
                print(allCollidersOnThisObjectAreTouchingDoor);
                if (allCollidersOnThisObjectAreTouchingDoor)
                {
                    print("All colliders on this object are touching a door!");
                    break; 
                }
                else
                {
                    yield return new WaitForSeconds(0.2f);
                    print("Rotating the room");
                    spawnedRoomWMoorDoors.transform.Rotate(Vector3.up, 90);
                }
            }
        }
        else if (doorsAroundRoom == 4 && doors.Length != 4)
        {
            StopCoroutine(SpawnRooms());
            Destroy(gameObject);
            GameObject spawnedRoomWMoorDoors = Instantiate(roomSpawner.room4Doors, transform.position, Quaternion.Euler(0, 0, 0));
            spawnedRoomWMoorDoors.GetComponent<Room>().AllowRoomSpawn = false;
        }

        //Destroy(GetComponent<BoxCollider>());
        //foreach (Transform door in doors)
        //{
        //    Destroy(door.gameObject);
        //}
        //Destroy(this);
    }
    private List<Vector3> doorPos = new();
    private List<Vector3> roomPos = new();
    private Vector3 roomSize;
    //private void OnDrawGizmos()
    //{
    //    foreach (Vector3 pos in roomPos)
    //    {

    //        Gizmos.DrawSphere(pos, 0.3f);
    //        //print(roomSize);
    //        //Gizmos.DrawCube(pos, roomSize);

    //    }
    //    foreach (Vector3 pos in doorPos)
    //    {
    //        Gizmos.DrawSphere(pos, 0.3f);
    //    }
    //}
}
