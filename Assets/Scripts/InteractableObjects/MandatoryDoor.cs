using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MandatoryDoor : InteractableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;

    public bool RoomIsCompleted { get; set; }
    private bool _isOpen;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private AudioSource _audioSource;

    private void Awake()
    {
        _isOpen = true;
        RoomIsCompleted = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>(); 
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    #region Unity Methods
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (RoomIsCompleted) base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (RoomIsCompleted) base.OnTriggerExit2D(collision);
    }
    #endregion

    protected override void Interact()
    {
        base.Interact();
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        _spriteRenderer.sprite = _isOpen ? _sprites[0] : _sprites[1];
        _collider.enabled = _isOpen;
        _isOpen = !_isOpen;

        if (_isOpen && _openSound != null)
        {
            _audioSource.PlayOneShot(_openSound);
        }
        else if (!_isOpen && _closeSound != null)
        {
            _audioSource.PlayOneShot(_closeSound);
        }
    }


}
