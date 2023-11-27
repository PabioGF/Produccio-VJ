using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractableObject
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;

    private bool isOpen;
    private bool isUnlocked;
    private bool _hasInteracted;

    protected override void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    protected override void Interact(PlayerController playerController)
    {
        if (isUnlocked)
        {
            ToggleDoor();
        }
        else
        {
            bool hasKey = playerController.HasItem(InventoryItem.ItemType.Key, id);
            playerController.RemoveItem(InventoryItem.ItemType.Key, id);
            Debug.Log(hasKey);
            if (hasKey)
            {
                isUnlocked = true;
                ToggleDoor();
            }
        }
    }

    private void ToggleDoor()
    {
        GetComponent<SpriteRenderer>().sprite = isOpen ? _closedSprite : _openSprite;
        GetComponent<Collider2D>().enabled = isOpen;
        isOpen = !isOpen;
    }

    /// <summary>
    /// Checks if the player enters the door trigger
    /// </summary>
    /// <param name="collision">Collision</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerInputActions.Player.Interact.ReadValue<float>() == 0) _hasInteracted = false;

        if (collision.TryGetComponent<PlayerController>(out var component) && _playerInputActions.Player.Interact.ReadValue<float>() == 1)
        {
            if (!_hasInteracted) Interact(component);
            _hasInteracted = true;
        }
    }
}
