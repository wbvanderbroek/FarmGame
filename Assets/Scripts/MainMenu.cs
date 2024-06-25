using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controls;
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }
    public void Options()
    {

    }
    public void EnableControls()
    {
        controls.SetActive(true);
    }
    public void DisableControls()
    {
        controls.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
