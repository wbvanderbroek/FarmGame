using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().canMove = false;
            if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene("Generation");
            }
            if (SceneManager.GetActiveScene().name == "Generation")
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}
