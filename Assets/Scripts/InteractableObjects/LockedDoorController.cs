using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LockedDoorController : InteractableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] protected int id;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Audio")]
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;
    [SerializeField] private float _openVolume = 1.0f;
    [SerializeField] private float _closeVolume = 1.0f;

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
        _sprite.sprite = _isOpen ? _sprites[0] : _sprites[1];
        _collider.enabled = _isOpen;
        _isOpen = !_isOpen;

        if (_isOpen && _openSound != null)
        {
            _audioSource.PlayOneShot(_openSound, _openVolume);
        }
        else if (!_isOpen && _closeSound != null) 
        {
            _audioSource.PlayOneShot(_closeSound, _closeVolume);
        }
    }

    public void UnlockDoor()
    {
        _isUnlocked = true;
    }
}