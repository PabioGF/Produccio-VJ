using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryItem
{
    protected ItemType _type;
    public enum ItemType { Key, Bottle, Shield }
    public ItemType Type => _type;
}
