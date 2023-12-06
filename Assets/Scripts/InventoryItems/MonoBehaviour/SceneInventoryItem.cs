using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInventoryItem : MonoBehaviour
{
    protected PlayerInputActions _playerInputActions;
    protected PlayerController _playerController;

    protected virtual void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }
    
    protected virtual void PickUp()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var component))
        {
            _playerController = component;
        }
            
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        _playerController = null;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerInputActions.Player.Interact.ReadValue<float>() == 1 && _playerController != null)
        {
            PickUp();
        }
    }
}
