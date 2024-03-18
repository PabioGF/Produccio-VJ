using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DoorController : InteractableObject
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;

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
        Debug.Log("Open/Close");
        GetComponent<SpriteRenderer>().sprite = isOpen ? _closedSprite : _openSprite;
        GetComponent<Collider2D>().enabled = isOpen;
        isOpen = !isOpen;

        if (isOpen && _openSound != null)
        {
            _audioSource.PlayOneShot(_openSound);
        }
        else if (!isOpen && _closeSound != null)
        {
            _audioSource.PlayOneShot(_closeSound);
        }
    }
}
