using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class NewBehaviourScript : ItemObject
{
  public int restoredHealthValue;
  public void Awake()
  {
    type = ItemType.Consumable;
  }
}
