using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
  public string savePath;
  public ItemDatabaseObject database;
  public Inventory Container;


  public void AddItem(Item _item, int _amount)
  {
    for (int i = 0; i < Container.Items.Count; i++)
    {
      if (_item == Container.Items[i].item)
      {
        Container.Items[i].AddAmount(_amount);
        return;
      }
    }
    Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));

  }
  [ContextMenu("Save")]
  public void Save()
  {
    /* 
    // save in Json format, more easily human readable
    string saveData = JsonUtility.ToJson(this, true);
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
    bf.Serialize(file, saveData);
    file.Close();
    */

    // save in Iformatter mode, less easaly readable by humans
    IFormatter formatter = new BinaryFormatter();
    Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
    formatter.Serialize(stream, Container);
    stream.Close();
  }

[ContextMenu("Load")]
  public void Load()
  {
    if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
    {
      /* 
    // load from Json format, more easily human readable
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
      JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
      file.Close();
      */
      // load from in Iformatter mode, less easaly readable by humans
      IFormatter formatter = new BinaryFormatter();
      Stream stream = new FileStream(string.Concat(Application.persistentDataPath,savePath), FileMode.Open, FileAccess.Read);
      Container = (Inventory)formatter.Deserialize(stream);
      stream.Close();
    }
  }
  [ContextMenu("Clear")]
  public void clear()
  {
    Container = new Inventory();
  }
}

[System.Serializable]
public class Inventory
{
  public List<InventorySlot> Items = new List<InventorySlot>();
}

[System.Serializable]
public class InventorySlot
{
  public Item item;
  public int amount;
  public int ID;
  public InventorySlot(int _id, Item _item, int _amount)
  {
    ID = _id;
    item = _item;
    amount = _amount;
  }
  public void AddAmount(int value)
  {
    amount += value;
  }
}
