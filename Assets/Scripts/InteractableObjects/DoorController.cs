using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractableObject
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;

    private bool isOpen;

    protected override void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    protected override void Interact(PlayerController playerController)
    {
        bool hasKey = playerController.HasItem(InventoryItem.ItemType.Key, id);
        Debug.Log("Has key");

        if (hasKey)
        {
            isOpen = true;
            //collision.GetComponent<PlayerController>().setKey(false);
            //GetComponent<SpriteRenderer>().sprite = _openSprite;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Debug.Log("Necesitas una llave para abrir esta puerta.");
        }
    }

    /// <summary>
    /// Checks if the player enters the door trigger
    /// </summary>
    /// <param name="collision">Collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var component) && _playerInputActions.Player.Interact.ReadValue<float>() == 1)
        {
            Interact(component);
        }
    }
}
