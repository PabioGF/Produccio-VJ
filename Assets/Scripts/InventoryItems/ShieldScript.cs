using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldScript : InventoryItem
{
    public override ItemType GetItemType()
    {
        return type;
    }

    public override int GetItemId()
    {
        return id;
    }

    protected override void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    protected override void PickUp(PlayerController playerController)
    {
        Debug.Log("Pick Shield");
        playerController.AddItem(GetComponent<InventoryItem>());
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var component) && _playerInputActions.Player.Interact.ReadValue<float>() == 1) 
        {
            PickUp(component);
        }
    }
}
