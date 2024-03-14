using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] doors;
    public Vector3 roomScale = new Vector3(1, 1, 1);
    private RoomSpawner roomSpawner;
    [SerializeField] private bool AllowRoomSpawn = true;

    private void Start()
    {
        roomSpawner = RoomSpawner.Instance;
        //StartCoroutine(SpawnRooms());
    }
    public IEnumerator SpawnRooms()
    {
        yield return new WaitForSeconds(1f);
        if (AllowRoomSpawn)
        {
            foreach (var door in doors)
            {
                int rnd = Random.Range(0, roomSpawner.roomPrefabs.Length);
                //if (transform.eulerAngles.y == 0)
                //{
                //    print("1");
                //}
                //else if (transform.eulerAngles.y == 90)
                //{
                //    door.SetPositionAndRotation(new Vector3(door.localPosition.x, door.localPosition.y, door.localPosition.z), door.rotation);
                //}
                //else if (transform.eulerAngles.y == 180)
                //{
                //    door.SetPositionAndRotation(new Vector3(door.localPosition.x * -1, door.localPosition.y, door.localPosition.x * -1), door.rotation);
                //}
                //else if (transform.eulerAngles.y == 270)
                //{
                //    door.SetPositionAndRotation(new Vector3(door.localPosition.x, door.localPosition.y, door.localPosition.z * -1), door.rotation);
                //}

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
                doorPos.Add(door.localPosition + transform.position);
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
                                break;
                            }
                            else
                            {
                                yield return new WaitForSeconds(1f);

                                spawnedRoom.transform.Rotate(Vector3.up, 90);
                            }
                        }
                        yield return new WaitForSeconds(1f);
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
                yield return new WaitForSeconds(1f);
            }
        }
    }
    private List<Vector3> doorPos = new();
    private List<Vector3> roomPos = new();
    private Vector3 roomSize;
    private void OnDrawGizmos()
    {
        foreach (Vector3 pos in roomPos)
        {

            Gizmos.DrawSphere(pos, 0.3f);
            //print(roomSize);
            //Gizmos.DrawCube(pos, roomSize);

        }
        foreach (Vector3 pos in doorPos)
        {
            Gizmos.DrawSphere(pos, 0.3f);
        }
    }
}
