using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LockedDoorController : InteractableObject
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;
    [SerializeField] protected int id;

    private bool isOpen;
    private bool _isUnlocked;
    private bool _hasInteracted;

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
        Debug.Log("Open/Close");
        GetComponent<SpriteRenderer>().sprite = isOpen ? _closedSprite : _openSprite;
        GetComponent<Collider2D>().enabled = isOpen;
        isOpen = !isOpen;
    }

    public void UnlockDoor()
    {
        _isUnlocked = true;
    }
}