using UnityEngine;

public class Crop : MonoBehaviour
{
    public enum growthStage {Growing, HarvestReady }
    public growthStage stage;

    public float growthTimer = 10;

    public ItemObject cropObject;

    public void Plant(ItemObject crop)
    {
        cropObject = crop;
        print(cropObject.name);
    }
    void Update()
    {
        if (growthTimer >0)
            growthTimer -= Time.deltaTime;
        else if (growthTimer <= 0)
        {
            if (stage == growthStage.Growing)
            {
                Vector3 newScale = transform.localScale;
                newScale.y *= 2;
                transform.localScale = newScale;
            }
            stage = growthStage.HarvestReady;
        }
    }
    public void Harvest()
    {
        Destroy(gameObject);
    }
}
