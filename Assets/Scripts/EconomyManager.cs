using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    private int coins = 100;
    private void Awake()
    {
        Instance = this;
    }

    public bool RemoveCoins(int _coins)
    {
        if((coins -= _coins) >= 0)
        {
            coins -= _coins;
            return true;
        }
        else
        {
            return false;
        }
    }
}
