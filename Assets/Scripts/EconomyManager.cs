using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    [SerializeField] TextMeshProUGUI coinText;

    public EconomyObject economyObject;



    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateText();
    }

    public bool RemoveCoins(int _coins)
    {
        if (economyObject.GetCoins >= _coins)
        {
            economyObject.GetCoins -= _coins;
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
        economyObject.GetCoins += _coins;
        UpdateText();
    }
    public void UpdateText()
    {
        coinText.text = economyObject.GetCoins.ToString();
    }
    public void Clear()
    {
        economyObject.Clear();
    }
}
