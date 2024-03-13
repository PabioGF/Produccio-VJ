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
        base.Interact();
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        Debug.Log("Open/Close");
        GetComponent<SpriteRenderer>().sprite = isOpen ? _closedSprite : _openSprite;
        GetComponent<Collider2D>().enabled = isOpen;
        isOpen = !isOpen;
    }
}
