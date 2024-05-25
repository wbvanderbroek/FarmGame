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

    void Start()
    {
        RoomGenerator.Instance.OnDone += SpawnBoss;

    }
    private void SpawnBoss()
    {
        boss = Instantiate(bossPrefab, transform.position, Quaternion.identity, gameObject.transform);
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
        StartCoroutine(bossDoor.OpenDoor());

    }
}
