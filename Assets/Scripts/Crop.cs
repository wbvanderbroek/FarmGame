using UnityEngine;

public class Crop : MonoBehaviour
{
    public enum growthStage {Growing, HarvestReady }
    public growthStage stage;

    public float growthTimer = 10;
    

    void Update()
    {
        if (growthTimer >0)
            growthTimer -= Time.deltaTime;
        else if (growthTimer <= 0)
        {
            if (stage == growthStage.Growing)
                transform.localScale = transform.localScale * 2;
            stage = growthStage.HarvestReady;
        }
    }

    public void Harvest()
    {
        Destroy(gameObject);
    }
}
