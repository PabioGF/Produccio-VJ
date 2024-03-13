using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MandatoryDoor : InteractableObject
{
    [SerializeField] private Sprite[] _sprites;

    public bool RoomIsCompleted { get; set; }
    private bool _isOpen;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private void Awake()
    {
        _isOpen = true;
        RoomIsCompleted = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
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
    }


}
