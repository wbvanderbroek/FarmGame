using System.Collections;
using UnityEngine;

public class Ore : MonoBehaviour
{
    private int oreHealth = 40;
    [SerializeField] private ItemObject oreObject;
    Vector3 initialScale;
    private float duration = 0.1f;
    private void Start()
    {
        initialScale = transform.localScale;
    }
    public void TakeDamage(int damage)
    {
        oreHealth = oreHealth - damage;
        if (oreHealth <= 0)
        {
            if (oreObject is OreObject _oreObject)
            {
                InventoryManager.Instance.AddItemToInventories(oreObject.data, _oreObject.amountToHarvest);
                Destroy(gameObject);
            }
        }

        StartCoroutine(ToSmallerSizeEffect());
    }
    private IEnumerator ToSmallerSizeEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, initialScale * 0.9f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ToBiggerSizeEffect());
    }
    private IEnumerator ToBiggerSizeEffect()
    {
        Vector3 initialScaleSmall = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScaleSmall, initialScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
