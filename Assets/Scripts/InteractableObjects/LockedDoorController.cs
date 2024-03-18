using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LockedDoorController : InteractableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] protected int id;
    [SerializeField] private AudioClip _openSound;

    private bool _isOpen;
    private bool _isUnlocked;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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

        if (_isOpen && _openSound != null)
        {
            _audioSource.PlayOneShot(_openSound);
        }
    }

    public void UnlockDoor()
    {
        _isUnlocked = true;
    }
}