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
        StartCoroutine(SpawnRooms());
    }
    IEnumerator SpawnRooms()
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
                Collider[] colliders = Physics.OverlapBox(pos2, GetComponent<BoxCollider>().bounds.extents / 2);
                if (colliders.Length == 0)
                {
                    if (roomSpawner.roomsLeftToSpawn > 0)
                    {
                        roomSpawner.roomsLeftToSpawn--;
                        int rnd = Random.Range(0, roomSpawner.roomPrefabs.Length);
                        Instantiate(roomSpawner.roomPrefabs[rnd], pos2, Quaternion.identity);
                    }
                    else if (roomSpawner.roomsLeftToSpawn == 0)
                    {
                        Instantiate(roomSpawner.endRoom, pos2, Quaternion.identity);
                    }

                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
