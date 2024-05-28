using System.Collections;
using UnityEngine;

public class LootChest : MonoBehaviour
{
    private Transform target; 
    private Vector3 targetAngle = new Vector3(-120, 0, 0);
    public Transform spawnPoint;
    public GameObject objectToSpawn;
    private int numberOfObjects = 40;
    private float spawnInterval = 0.075f;
    private float forceStrength = 8f;
    private float horizontalForceRange = 2f; 


    void Start()
    {
        target = transform.GetChild(0);
        StartCoroutine(Open(target, Quaternion.Euler(targetAngle), 2f));
    }

    IEnumerator Open(Transform target, Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = target.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.localRotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localRotation = targetRotation;
        StartCoroutine(SpawnObjects());
    }
    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate a random horizontal direction
                float randomX = Random.Range(-horizontalForceRange, horizontalForceRange);
                float randomZ = Random.Range(-horizontalForceRange, horizontalForceRange);

                Vector3 force = new Vector3(randomX, forceStrength, randomZ);
                rb.AddForce(force, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
