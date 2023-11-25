using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private ObjectType type;
    [SerializeField] protected int id;

    public enum ObjectType { Door, Lever }
    protected PlayerInputActions _playerInputActions;

    protected abstract void Awake();
    protected abstract void Interact(PlayerController playerController);
}