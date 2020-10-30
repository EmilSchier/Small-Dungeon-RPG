using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class displayInventory : MonoBehaviour
{
  public GameObject inventoryPrefab;
  public InventoryObject inventory;
  public int xStart;
  public int yStart;
  public int xSpaceBetweenItems;
  public int ySpaceBetweenItems;
  public int NumberOfColumns;

  Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
  // Start is called before the first frame update
  void Start()
  {
    CreateDisplay();
  }

  // Update is called once per frame
  void Update()
  {
    UpdateDisplay();
  }

  public void CreateDisplay()
  {
    for (int i = 0; i < inventory.Container.Items.Count; i++)
    {
      InventorySlot slot = inventory.Container.Items[i];

      var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
      obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
      obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
      obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
      itemsDisplayed.Add(slot, obj);
    }
  }

  public void UpdateDisplay()
  {
    for (int i = 0; i < inventory.Container.Items.Count; i++)
    {
      InventorySlot slot = inventory.Container.Items[i];

      if (itemsDisplayed.ContainsKey(slot))
      {
        itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
      }
      else
      {
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        itemsDisplayed.Add(slot, obj);
      }
    }
  }

  public Vector3 GetPosition(int i)
  {
    return new Vector3(xStart + xSpaceBetweenItems * (i % NumberOfColumns), yStart + (-ySpaceBetweenItems * (i / NumberOfColumns)), 0f);
  }
}

