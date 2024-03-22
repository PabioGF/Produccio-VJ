using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DoorController : InteractableObject
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Audio")]
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;
    [SerializeField] private float _openVolume = 1.0f; 
    [SerializeField] private float _closeVolume = 1.0f;

    private bool isOpen;
    private bool _hasInteracted;
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
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        _sprite.sprite = isOpen ? _closedSprite : _openSprite;
        _collider.enabled = isOpen;
        isOpen = !isOpen;

        if (isOpen && _openSound != null)
        {
            _audioSource.PlayOneShot(_openSound, _openVolume);
        }
        else if (!isOpen && _closeSound != null)
        {
            _audioSource.PlayOneShot(_closeSound, _closeVolume);
        }
    }
}
