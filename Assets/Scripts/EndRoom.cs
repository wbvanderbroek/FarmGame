using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoom : MonoBehaviour
{
    [SerializeField] private BossDoor bossDoor;
    [SerializeField] private GameObject bossPrefab;
    private GameObject boss;
    [SerializeField] private GameObject lootChest;

    void Start()
    {
        boss = Instantiate(bossPrefab, transform.position, Quaternion.identity, gameObject.transform);
    }

    void Update()
    {
        if (bossDoor.doorState == DoorState.Closed)
        {
            boss.GetComponent<Boss>().playerIsInRoom = true;
        }
    }
}
