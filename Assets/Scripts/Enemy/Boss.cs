using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject slamAttackObject;
    public bool playerIsInRoom = false;

    //triggered within animation
    private void SlamAttack()
    {
        //add camera shake 1!!!!!1!!11!!11!1


        // Spawn slamAttackObject in a ring around the Boss
        int numberOfObjects = 35; // Number of objects to spawn
        float radius = 7f; // Radius of the ring

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate the angle for each object
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                0 + 0.5f, // Assuming you want to spawn the objects at the same height as the Boss
                Mathf.Sin(angle) * radius
            );

            // Convert local position to world position
            spawnPosition += transform.position;

            // Instantiate the slamAttackObject at the calculated position
            GameObject spawnedObject = Instantiate(slamAttackObject, spawnPosition, Quaternion.identity);

            // Calculate the direction from the Boss to the spawn position
            Vector3 direction = spawnPosition - transform.position;

            // Initialize the spawned object with the movement direction
            spawnedObject.GetComponent<SlamAttackObject>().Initialize(direction);
        }
    }


}
