using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryItem : MonoBehaviour
{
    [SerializeField] protected ItemType type;
    [SerializeField] protected int id = 0;

    public enum ItemType { Key, Shield }
    protected PlayerInputActions _playerInputActions;

    protected abstract void Awake();
    protected abstract void PickUp(PlayerController playerController);
    public abstract ItemType GetItemType();
    public abstract int GetItemId();
}
