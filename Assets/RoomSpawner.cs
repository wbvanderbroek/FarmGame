using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject startRoom;

    private void Start()
    {
        StartCoroutine(SpawnRooms());
    }
    //void Start()
    //{
    //    Instantiate(startRoom);
    //    //Vector3 scale = new Vector3(
    //    //    startRoom.GetComponent<Room>().doors[0].position.x * startRoom.GetComponent<Room>().roomScale.x,
    //    //    startRoom.GetComponent<Room>().doors[0].position.y * startRoom.GetComponent<Room>().roomScale.y,
    //    //    startRoom.GetComponent<Room>().doors[0].position.z * startRoom.GetComponent<Room>().roomScale.z);
    //    //scale *= 2;
    //    //Vector3 pos = transform.position + scale;
    //    //Instantiate(roomPrefabs[0], pos, Quaternion.identity);

    //    foreach (var door in startRoom.GetComponent<Room>().doors)
    //    {
    //        Vector3 scale = new Vector3(
    //            door.position.x * startRoom.GetComponent<Room>().roomScale.x,
    //            door.position.y * startRoom.GetComponent<Room>().roomScale.y,
    //            door.position.z * startRoom.GetComponent<Room>().roomScale.z);
    //        scale *= 2;
    //        Vector3 pos = startRoom.transform.position + scale;
    //        GameObject room = Instantiate(roomPrefabs[0], pos, Quaternion.identity);
    //        for (int i = 0; i < 3; i++)
    //        {
    //            foreach (var door2 in room.GetComponent<Room>().doors)
    //            {
    //                Vector3 scale2 = new Vector3(
    //                    door2.position.x * room.GetComponent<Room>().roomScale.x,
    //                    door2.position.y * room.GetComponent<Room>().roomScale.y,
    //                    door2.position.z * room.GetComponent<Room>().roomScale.z);
    //                scale *= 2;
    //                Vector3 pos2 = room.transform.position + scale;
    //                Instantiate(roomPrefabs[0], pos2, Quaternion.identity);

    //            }
    //        }
    //    }
    //}
    IEnumerator SpawnRooms()
    {
        Instantiate(startRoom);
        //Vector3 scale = new Vector3(
        //    startRoom.GetComponent<Room>().doors[0].position.x * startRoom.GetComponent<Room>().roomScale.x,
        //    startRoom.GetComponent<Room>().doors[0].position.y * startRoom.GetComponent<Room>().roomScale.y,
        //    startRoom.GetComponent<Room>().doors[0].position.z * startRoom.GetComponent<Room>().roomScale.z);
        //scale *= 2;
        //Vector3 pos = transform.position + scale;
        //Instantiate(roomPrefabs[0], pos, Quaternion.identity);

        foreach (var door in startRoom.GetComponent<Room>().doors)
        {
            Vector3 scale = new Vector3(
                door.position.x * startRoom.GetComponent<Room>().roomScale.x,
                door.position.y * startRoom.GetComponent<Room>().roomScale.y,
                door.position.z * startRoom.GetComponent<Room>().roomScale.z);
            scale *= 2;
            Vector3 pos = startRoom.transform.position + scale;
            GameObject room = Instantiate(roomPrefabs[0], pos, Quaternion.identity);
            //for (int i = 0; i < 1; i++)
            //{

            //    //foreach (var door2 in room.GetComponent<Room>().doors)
            //    //{
            //        Vector3 scale2 = new Vector3(
            //            room.GetComponent<Room>().doors[0].localPosition.x * room.GetComponent<Room>().roomScale.x,
            //            room.GetComponent<Room>().doors[0].localPosition.y * room.GetComponent<Room>().roomScale.y,
            //            room.GetComponent<Room>().doors[0].localPosition.z * room.GetComponent<Room>().roomScale.z);
            //        scale2 *= 2;
            //        Vector3 pos2 = room.transform.position + scale2;
            //        Instantiate(roomPrefabs[0], pos2, Quaternion.identity);
            //        yield return new WaitForSeconds(1f);

            //    //}
            //}
            yield return new WaitForSeconds(1f);


        }
    }

    void Update()
    {
        
    }
}
