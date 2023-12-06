using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key: InventoryItem
{
    private int _id;

    public Key(int id)
    {
        _type = ItemType.Key;
        this._id = id;
    }

    public int Id => _id;
}
