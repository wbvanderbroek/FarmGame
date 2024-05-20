using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineDoor : MonoBehaviour
{
    [SerializeField] private Animator transition;
    private void Start()
    {
        transition = GameObject.Find("CanvasCrossfade").GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().canMove = false;
            StartCoroutine(LoadScene());
        }
    }
    private IEnumerator LoadScene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
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
