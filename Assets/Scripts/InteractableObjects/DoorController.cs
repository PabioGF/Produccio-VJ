using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DoorController : InteractableObject
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;

    private bool isOpen;
    private bool _hasInteracted;

    protected override void Interact()
    {
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        Debug.Log("Open/Close");
        GetComponent<SpriteRenderer>().sprite = isOpen ? _closedSprite : _openSprite;
        GetComponent<Collider2D>().enabled = isOpen;
        isOpen = !isOpen;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerInputActions.Player.Interact.ReadValue<float>() == 0) _hasInteracted = false;

        if (_playerInputActions.Player.Interact.ReadValue<float>() == 1 && _playerController != null)
        {
            if (!_hasInteracted) Interact();
            _hasInteracted = true;
        }
    }
}
