using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInventoryFix : MonoBehaviour
{
    public GameObject GO1;
    public GameObject GO2;
    void Start()
    {
        GO1.SetActive(true);
        GO2.SetActive(true);


    }

    public float targetTime = 0.1f;

    void Update()
    {

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            GO1.SetActive(false);
            GO2.SetActive(false);
            this.GetComponent<TempInventoryFix>().enabled = false;
        }

    }
}
