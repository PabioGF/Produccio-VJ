using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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

    public bool HasItem(InventoryItem.ItemType type)
    {
        foreach (InventoryItem item in _inventoryItems)
        {
            if (item.Type == type)
                return true;
        }
        return false;
    }

    public bool HasKey(int id)
    {
        foreach (InventoryItem item in _inventoryItems)
        {
            if (item.Type == InventoryItem.ItemType.Key)
            {
                Key key = (Key)item;
                if (key.Id == id) return true;
            }
        }
        return false;
    }

    public void RemoveItem(InventoryItem.ItemType type)
    {
        foreach (InventoryItem item in _inventoryItems)
        {
            if (item.Type == type)
            {
                _inventoryItems.Remove(item);
                return;
            }
        }
    }

    public void RemoveKey(int id)
    {
        foreach (InventoryItem item in _inventoryItems)
        {
            if (item.Type == InventoryItem.ItemType.Key)
            {
                Key key = (Key)item;
                if (key.Id == id)
                {
                    _inventoryItems.Remove(item);
                    return;
                }
            }
        }
    }
}
