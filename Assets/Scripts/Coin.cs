using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Rigidbody>() != null)
        {

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(GetComponent<Rigidbody>());
        }

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            EconomyManager.Instance.AddCoins(1);
        }
    }
}
