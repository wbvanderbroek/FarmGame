using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    [SerializeField] TextMeshProUGUI coinText;
    private int coins = 10000;
    private void Awake()
    {
        Instance = this;
    }

    public bool RemoveCoins(int _coins)
    {
        if(coins >= _coins)
        {
            coins -= _coins;
            UpdateText();
            return true;
        }
        else
        {
            return false;
        }

    }
    public void AddCoins(int _coins)
    {
        coins += _coins;
        UpdateText();
    }
    private void UpdateText()
    {
        coinText.text = coins.ToString();
    }
}
