using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private float targetHeight = -1.43f;
    private float closeTime = 1.0f;
    Transform door;
    public DoorState doorState = DoorState.Open; 

    private void Start()
    {
        door = transform.GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && doorState == DoorState.Open)
        {
            StartCoroutine(CloseDoorRoutine());
        }
    }

    private IEnumerator CloseDoorRoutine()
    {
        Vector3 initialPosition = door.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, targetHeight, initialPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < closeTime)
        {
            door.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / closeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        print("closed");
        doorState = DoorState.Closed;
        door.position = targetPosition;
    }
    public IEnumerator OpenDoor()
    {
        Vector3 initialPosition = door.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, -targetHeight, initialPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < closeTime)
        {
            door.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / closeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        door.position = targetPosition;
    }
}
public enum DoorState
{
    Open,
    Closed
}

