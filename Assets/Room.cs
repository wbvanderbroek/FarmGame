using System.Collections;
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
            foreach (var door2 in doors)
            {
                Vector3 scale2 = new Vector3(
                    door2.localPosition.x * roomScale.x,
                    door2.localPosition.y * roomScale.y,
                    door2.localPosition.z * roomScale.z);
                scale2 *= 2;
                Vector3 pos2 = transform.position + scale2;
                Collider[] colliders = Physics.OverlapBox(pos2, GetComponent<BoxCollider>().bounds.extents / 1.01f, Quaternion.identity, ~(1 << LayerMask.NameToLayer("Door")));
                if (colliders.Length == 0)
                {
                    if (roomSpawner.roomsLeftToSpawn > 0)
                    {
                        roomSpawner.roomsLeftToSpawn--;
                        int rnd = Random.Range(0, roomSpawner.roomPrefabs.Length);
                        GameObject spawnedRoom = Instantiate(roomSpawner.roomPrefabs[rnd], pos2, Quaternion.identity);
                        StartCoroutine(spawnedRoom.GetComponent<Room>().SpawnRooms());
                    }
                    else if (roomSpawner.roomsLeftToSpawn == 0)
                    {
                        GameObject lastRoom = Instantiate(roomSpawner.endRoom, pos2, Quaternion.identity);

                        while (door2.position != lastRoom.GetComponent<Room>().doors[0].position)
                        {
                            yield return new WaitForSeconds(1f);

                            lastRoom.transform.Rotate(Vector3.up, 90);
                        }
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
