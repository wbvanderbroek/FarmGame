using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "New Economy Object", menuName = "Inventory/Economy")]
public class EconomyObject : ScriptableObject
{
    public string savePath = "/Economy.Save";
    public Economy economy;
    public int GetCoins { get { return economy.coins; } set { economy.coins = value; } }
    [ContextMenu("Createfile")]
    public void CreateFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
            return;
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(stream, economy);
        stream.Close();
    }
    [ContextMenu("Save")]
    public void Save()
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(stream, economy);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.OpenOrCreate, FileAccess.Read);
            Economy _ecoObject = (Economy)formatter.Deserialize(stream);
            economy = _ecoObject;
            EconomyManager.Instance.UpdateText();

            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        economy.coins = 50;
    }
}
[System.Serializable]
public class Economy
{
    public int coins = 10000;
}
