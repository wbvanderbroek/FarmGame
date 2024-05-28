using System.Collections;
using UnityEngine;

public class LootChest : MonoBehaviour
{
    public Transform target; // The target transform to rotate
    private Vector3 targetAngle = new Vector3(-120, 0, 0);

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
    }
    private void SpawnCoins()
    {

    }
}
