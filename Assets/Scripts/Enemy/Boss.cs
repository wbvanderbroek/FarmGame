using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject slamAttackObject;
    public bool playerIsInRoom = false;
    [SerializeField] private Image healthBar;
    [SerializeField] private AudioSource audioBossIn;
    [SerializeField] private GameObject groundSpark;
    private void Start()
    {
        StartCoroutine(CheckIfPlayerIsInRoom());
    }
    private IEnumerator InAnimation()
    {
        Vector3 initialPosition = transform.position;
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        Vector3 targetPosition = new Vector3(initialPosition.x, transform.position.y - 8f, initialPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < 3)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / 3);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }
    private IEnumerator CheckIfPlayerIsInRoom()
    {
        while (!playerIsInRoom)
        {
            yield return null;
        }

        healthBar.transform.parent.gameObject.SetActive(true);
        StartCoroutine(CheckHealth());
        StartCoroutine(InAnimation());
        audioBossIn.Play();
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

        Instantiate(groundSpark, transform.position, Quaternion.identity);

        int numberOfObjects = 30;
        float radius = 4f;

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate the angle for each object
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                0 + 0.5f,
                Mathf.Sin(angle) * radius
            );

            spawnPosition += new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            GameObject spawnedObject = Instantiate(slamAttackObject, spawnPosition, Quaternion.identity);

            Vector3 direction = spawnPosition - transform.position;
            spawnedObject.GetComponent<SlamAttackObject>().Initialize(direction);
        }
    }
    public void UpdateHealth()
    {
        healthBar.fillAmount = GetComponent<Enemy>().health / GetComponent<Enemy>().maxHealth;
    }
}
