using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyScript : InventoryItem
{
    protected override void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += InteractInput;
    }

    protected override void OnDestroy()
    {
        _playerInputActions.Player.Jump.performed -= InteractInput;
    }

    protected override void PickUp(PlayerController playerController)
    {
        Debug.Log("PickKey");
        //playerController.setKey(true);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            _collider = collision;
            _playerContact = true;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collider = null;
            _playerContact = false;
        }
    }
    public override void InteractInput(InputAction.CallbackContext context)
    {
        if (_collider.TryGetComponent<PlayerController>(out var component))
        {
            PickUp(component);
        }
    }
}
