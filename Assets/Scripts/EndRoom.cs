using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndRoom : MonoBehaviour
{
    [SerializeField] private BossDoor bossDoor;
    [SerializeField] private GameObject bossPrefab;
    private GameObject boss;
    [SerializeField] private GameObject lootChest;

    void Awake()
    {
        RoomGenerator.Instance.OnDone += SpawnBoss;
    }
    private void SpawnBoss()
    {
        boss = Instantiate(bossPrefab, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity, gameObject.transform);
        StartCoroutine(DoorStateChecker());
        StartCoroutine(DeadChecker());
    }
    private IEnumerator DoorStateChecker()
    {
        while (bossDoor.doorState != DoorState.Closed)
        {
            yield return null;
        }
        boss.GetComponent<Boss>().playerIsInRoom = true;
    }
    private IEnumerator DeadChecker()
    {
        while (boss != null)
        {
            yield return null;
        }
        Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
        Instantiate(lootChest, new Vector3(transform.position.x, transform.position.y -3.1f, transform.position.z), rotation);
        StartCoroutine(bossDoor.OpenDoor());

    }
}
