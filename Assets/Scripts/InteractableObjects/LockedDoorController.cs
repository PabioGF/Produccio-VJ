using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LockedDoorController : InteractableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] protected int id;

    private bool _isOpen;
    private bool _isUnlocked;

    protected override void Interact()
    {
        base.Interact();
        if (_isUnlocked)
        {
            ToggleDoor();
        }
        else
        {
            bool hasKey = _playerController.HasItem(InventoryItem.ItemType.Key, id);
            _playerController.RemoveItem(InventoryItem.ItemType.Key, id);
            Debug.Log(hasKey);
            if (hasKey)
            {
                _isUnlocked = true;
                ToggleDoor();
            }
        }
    }

    private void ToggleDoor()
    {
        GetComponent<SpriteRenderer>().sprite = _isOpen ? _sprites[0] : _sprites[1];
        GetComponent<Collider2D>().enabled = _isOpen;
        _isOpen = !_isOpen;
    }

    public void UnlockDoor()
    {
        _isUnlocked = true;
    }
}