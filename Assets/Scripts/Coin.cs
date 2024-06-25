using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        EconomyManager.Instance.AddCoins(1);
        StartCoroutine(DestroyCoin());
    }
    private IEnumerator DestroyCoin()
    {

        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
    }
}
