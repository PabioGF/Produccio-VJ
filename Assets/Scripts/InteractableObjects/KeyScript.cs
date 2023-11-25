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
        //playerController.setKey(true);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var component) && Input.GetKeyDown(KeyCode.E))
        {
            PickUp(component);
        }
    }
    public override void InteractInput(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }
}
