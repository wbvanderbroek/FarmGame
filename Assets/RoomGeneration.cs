using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    [SerializeField] private Vector2 gridSize = new Vector2(10,10);
    [SerializeField] private GameObject[] roomPrefabs;
    private Vector3 currentRoomPos;
    [SerializeField] private Vector2 roomSize = new Vector2(1,1);
    [SerializeField] private LayerMask doorLayer;
    private void Start()
    {
         StartCoroutine(GenerateRooms());
    }
    private IEnumerator GenerateRooms()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            currentRoomPos.x += roomSize.x;
            for(int j = 0; j < gridSize.y; j++)
            {
                currentRoomPos.z += roomSize.y;
                SpawnRoom(currentRoomPos);
                yield return new WaitForSeconds(0.1f);
            }
            currentRoomPos.z = 0;
        }
    }
    private void SpawnRoom(Vector3 pos)
    {
        //int randomRoom = Random.Range(0, roomPrefabs.Length);


        foreach (var room in roomPrefabs)
        {
            Room _room = room.GetComponent<Room>();
            if (CanPlace(_room, pos))
            {

                Instantiate(room, pos, Quaternion.identity);
                return;
            }
        }
    }
    private bool CanPlace(Room room, Vector3 pos)
    {
        foreach (var door in room.doors)
        {
            Vector3 doorPos = door.transform.position + pos;
            float doorRadius = 0.5f; // Adjust this based on your door size or desired buffer

            // Check for overlap with any collider (excluding door layer)
            Collider[] colliders = Physics.OverlapSphere(doorPos, doorRadius, ~doorLayer); // Invert doorLayer for exclusion
            if (colliders.Length > 0)
            {
                return false;
            }
        }
        return true;
    }

    private List<Vector3> doorPos = new();
    private void OnDrawGizmos()
    {
        foreach(Vector3 pos in doorPos)
        {
            Gizmos.DrawSphere(pos, 1f);
        }
    }
}
