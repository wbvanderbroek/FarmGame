using UnityEngine;

public class TempInventoryFix : MonoBehaviour
{
    public float targetTime = 0.1f;

    public GameObject GO1;
    public GameObject GO2;
    void Start()
    {
        GO1.SetActive(true);
        GO2.SetActive(true);
    }
    void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0.0f)
        {
            GO1.SetActive(false);
            GO2.SetActive(false);
            GetComponent<TempInventoryFix>().enabled = false;
        }
    }
}
