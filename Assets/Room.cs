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
        //foreach (var door2 in doors)
        //{
        //    Vector3 scale2 = new Vector3(
        //        door2.localPosition.x * roomScale.x,
        //        door2.localPosition.y * roomScale.y,
        //        door2.localPosition.z * roomScale.z);
        //    doorPos.Add(transform.position + scale2);
        //}
        yield return new WaitForSeconds(1f);
        if (AllowRoomSpawn)
        {
            foreach (var door in doors)
            {
                Vector3 scale = new Vector3(
                    door.localPosition.x * roomScale.x,
                    door.localPosition.y * roomScale.y,
                    door.localPosition.z * roomScale.z);
                scale *= 2;
                Vector3 pos = transform.position + scale;
                int rnd = Random.Range(0, roomSpawner.roomPrefabs.Length);

                Collider[] colliders = Physics.OverlapBox(pos, roomSpawner.roomPrefabs[rnd].GetComponent<BoxCollider>().bounds.extents / 1.01f, Quaternion.identity, ~(1 << LayerMask.NameToLayer("Door")));
                if (colliders.Length == 0)
                {
                    if (roomSpawner.roomsLeftToSpawn > 0)
                    {
                        roomSpawner.roomsLeftToSpawn--;
                        GameObject spawnedRoom = Instantiate(roomSpawner.roomPrefabs[rnd], pos, Quaternion.identity);

                        for (int i = 0; i < 10; i++)
                        {
                            Collider[] colliders2 = Physics.OverlapSphere(door.position, 1f, LayerMask.GetMask("Door"));
                            foreach (Collider collider2 in colliders2)
                            {
                                print(collider2.transform.parent.name);
                            }
                            if (colliders2.Length > 1)
                            {

                                if ((colliders2[0].transform.parent.gameObject == colliders2[1].transform.parent.gameObject)
                                    || (colliders2[1].transform.parent.gameObject == colliders2[0].transform.parent.gameObject))
                                {
                                    print(colliders2[0].transform.position + "    " + colliders2[1].transform.position);

                                    yield return new WaitForSeconds(1f);
                                    break;
                                }
                                else
                                {
                                    //yield return new WaitForSeconds(1f);
                                    spawnedRoom.transform.Rotate(Vector3.up, 90);
                                }
                            }
                            else
                            {
                                yield return new WaitForSeconds(1f);
                                spawnedRoom.transform.Rotate(Vector3.up, 90);

                            }


                        }
                        yield return new WaitForSeconds(1f);
                        doorPos.Add(transform.position + scale);

                        StartCoroutine(spawnedRoom.GetComponent<Room>().SpawnRooms());


                    }
                    else if (roomSpawner.roomsLeftToSpawn == 0)
                    {
                        GameObject lastRoom = Instantiate(roomSpawner.endRoom, pos, Quaternion.identity);
                        bool doorFound = false;
                        while (!doorFound)
                        {
                            foreach (var door2 in lastRoom.GetComponent<Room>().doors)
                            {
                                if (door.position == door2.position)
                                {
                                    doorFound = true;
                                    lastRoom.transform.Rotate(Vector3.up, 90);

                                    break;
                                }
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
    private void OnDrawGizmos()
    {
        foreach (Vector3 pos in doorPos)
        {
            Gizmos.DrawSphere(pos, 0.3f);
        }
    }
}
