using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject slamAttackObject;
    public bool playerIsInRoom = false;
    private void Start()
    {
        StartCoroutine(CheckHealth());
    }
    private IEnumerator CheckHealth()
    {
        while (GetComponent<Enemy>().health > GetComponent<Enemy>().maxHealth / 2)
        {
            yield return null;
        }
        GetComponent<Animator>().SetTrigger("Enraged");
    }
    //triggered within animation
    private void SlamAttack()
    {
        //add camera shake 1!!!!!1!!11!!11!1


        int numberOfObjects = 30;
        float radius = 2f; 

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate the angle for each object
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                0 + 0.5f, 
                Mathf.Sin(angle) * radius
            );

            spawnPosition += transform.position;
            GameObject spawnedObject = Instantiate(slamAttackObject, spawnPosition, Quaternion.identity);

            Vector3 direction = spawnPosition - transform.position;
            spawnedObject.GetComponent<SlamAttackObject>().Initialize(direction);
        }
    }
}
