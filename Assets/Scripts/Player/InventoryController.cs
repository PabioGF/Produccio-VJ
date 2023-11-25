using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private List<InventoryItem> _inventoryItems;

    void Awake()
    {
        _inventoryItems = new List<InventoryItem>();
    }

    /// <summary>
    /// Adds a new item to the inventory
    /// </summary>
    /// <param name="item">Item to add</param>
    public void AddItem(InventoryItem item)
    {
        _inventoryItems.Add(item);
        Debug.Log(_inventoryItems.Count);
    }

    public bool HasItem(InventoryItem.ItemType type, int id)
    {
        foreach (InventoryItem item in _inventoryItems)
        {
            if (item.GetItemType() == type && item.GetItemId() == id)
                return true;
        }
        return false;
    }

    public void RemoveItem()
    {

    }
}
