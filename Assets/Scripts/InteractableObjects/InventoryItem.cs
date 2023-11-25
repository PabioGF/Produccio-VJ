using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryItem : MonoBehaviour
{
    protected enum ItemType { Key, Lever }
    protected PlayerInputActions _playerInputActions;
    protected bool _playerContact;
    protected Collider2D _collider;

    protected abstract void Awake();
    protected abstract void OnDestroy();
    protected abstract void PickUp(PlayerController playerController);
    public abstract void InteractInput(InputAction.CallbackContext context);
}
