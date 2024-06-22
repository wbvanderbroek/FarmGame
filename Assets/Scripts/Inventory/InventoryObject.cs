using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }
    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCount <= 0)
            return false;

        InventorySlot slot = FindItemOnInventory(_item, !database.ItemObjects[_item.Id].isStackable);

        if (!database.ItemObjects[_item.Id].isStackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }

        //check if item can stack to another slot
        if (slot.amount + _amount <= 99)
        {
            slot.AddAmount(_amount);
            return true;
        }
        else
        {
            int remainingAmount = (slot.amount + _amount) - 99;
            slot.UpdateSlot(_item, 99);
            if (remainingAmount > 0)
            {
                InventorySlot slot2 = FindItemOnInventory(_item, database.ItemObjects[_item.Id].isStackable, remainingAmount, slot);
                if (slot2.item.Id == -1)
                {
                    SetEmptySlot(_item, remainingAmount);
                }
                else
                {
                    slot2.AddAmount(remainingAmount);
                    int itemsleft = slot2.amount - 99;

                    slot2.UpdateSlot(_item, Mathf.Clamp(slot2.amount, 0, 99));
                    if (itemsleft > 0)
                    {
                        AddItem(_item, itemsleft);
                    }
                }
                return true;
            }
            return false;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item, bool stacklimit = false, int _remainingAmount = 0, InventorySlot excludeSlot = null)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i] == excludeSlot)
                continue;
            if (stacklimit)
            {
                if (_remainingAmount > 0 && _remainingAmount + GetSlots[i].amount <= 99)
                {
                    return GetSlots[i];
                }
                if (GetSlots[i].amount < 99)
                {
                    return GetSlots[i];
                }
            }
            if (GetSlots[i].item.Id == _item.Id && !stacklimit)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        return null;
    }
    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1.item.Id == -1 && item2.item.Id == -1) return;
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            // Check if items are stackable and have the same ID
            if (item1.item.Id == item2.item.Id && item1.ItemObject.isStackable && (item1.amount != 99 && item2.amount != 99))
            {
                // Combine stacks, leaving the smaller stack empty
                int totalAmount = item1.amount + item2.amount;
                if (totalAmount <= 99)
                {
                    item2.UpdateSlot(item2.item, totalAmount);
                    item1.UpdateSlot(new Item(), 0);
                }
                else
                {
                    int remainingAmount = totalAmount - 99;
                    item2.UpdateSlot(item1.item, 99);
                    item1.UpdateSlot(item1.item, remainingAmount);
                }
            }
            else
            {
                // Not stackable or different IDs, swap normally
                InventorySlot temp = new InventorySlot(item2.item, item2.amount);
                item2.UpdateSlot(item1.item, item1.amount);
                item1.UpdateSlot(temp.item, temp.amount);
            }
        }
    }
    public void SplitItems(InventorySlot item1, InventorySlot item2)
    {
        int halfAmount = Mathf.CeilToInt(item1.amount / 2f);

        if (item2.item.Id == -1)
        {
            item2.UpdateSlot(item1.item, item1.amount - halfAmount);
            item1.UpdateSlot(item1.item, halfAmount);
        }
        else if (item1.item.Id == item2.item.Id && item2.amount < 99)
        {
            int halfAmountRoundDown = Mathf.FloorToInt(item1.amount / 2f);
            int totalAmount = halfAmount + item2.amount;

            if (totalAmount <= 99)
            {
                if (halfAmount > halfAmountRoundDown)
                {
                    totalAmount--;
                    item2.UpdateSlot(item1.item, totalAmount);
                    item1.UpdateSlot(item1.item, halfAmount);
                }
                else
                {
                    item2.UpdateSlot(item1.item, totalAmount);
                    item1.UpdateSlot(item1.item, halfAmount);
                }
            }
            else
            {
                int remainingAmount = totalAmount - 99;
                item2.UpdateSlot(item1.item, 99);
                item1.UpdateSlot(item1.item, (item1.amount - halfAmount) + remainingAmount);
            }
        }
    }
    public bool RemoveItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == _item)
            {
                GetSlots[i].amount--;
                if (GetSlots[i].amount <= 0)
                {
                    GetSlots[i].UpdateSlot(null, 0);
                    return true;
                }
                else
                {
                    GetSlots[i].UpdateSlot(_item, GetSlots[i].amount);
                    return true;
                }
            }
        }
        return false;
    }
    public void CreateFile(int _ID = 0)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + _ID)))
            return;
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _ID), FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    [ContextMenu("Save")]
    public void Save(int _ID = 0)
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _ID), FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load(int _ID = 0)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + _ID)))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _ID), FileMode.OpenOrCreate, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}
[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[36];
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}
public delegate void SlotUpdated(InventorySlot _slot);
[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    public Item item = new Item();
    public int amount;
    public ItemObject ItemObject
    {
        get
        {
            if (item.Id >= 0)
            {
                if (parent == null)
                {
                    Debug.Log(" is null");
                }
                return parent.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }
    public void UpdateSlot(Item _item, int _amount)
    {

        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }

        if (_amount <= 0)
        {
            item = new Item();
            amount = _amount;
        }
        else
        {
            item = _item;
            amount = _amount;
        }

        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }
    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
        {
            return true;
        }
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
            {
                return true;
            }
        }
        return false;
    }
    
}
