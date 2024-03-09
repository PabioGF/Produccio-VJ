using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private ObjectType type;

    public enum ObjectType { Door, LockedDoor, Lever, Elevator, Platform, HealingItem }
    protected PlayerController _playerController;

    protected virtual void Interact() 
    {
        _playerController.DesiredInteraction = false;
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
        _playerController = null;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerController != null && _playerController.DesiredInteraction)
        {
            Interact();
        }
    }

}