using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array to hold room prefabs
    public int numberOfRooms; // Number of rooms to generate
    public float roomWidth = 20f; // Width of each room
    public float roomHeight = 10f; // Height of each room
    public float corridorWidth = 5f; // Width of corridors between rooms

    void Start()
    {
        GenerateRooms();
    }

    void GenerateRooms()
    {
        Vector3 spawnPosition = Vector3.zero; // Initial spawn position

        // Loop through each room
        for (int i = 0; i < numberOfRooms; i++)
        {
            // Instantiate a random room prefab
            GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            GameObject room = Instantiate(roomPrefab, spawnPosition, Quaternion.identity);

            // Update spawn position for the next room
            spawnPosition.x += roomWidth + corridorWidth;

        }
    }
}
