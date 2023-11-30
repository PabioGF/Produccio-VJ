using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : InteractableObject
{
    #region Global Variables
    [SerializeField] private GameObject _linkedTurret;

    private bool _hasInteracted;
    #endregion


    protected override void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    protected override void Interact(PlayerController playerController)
    {
       _linkedTurret.SetActive(false);
    }

    /// <summary>
    /// Checks if the player enters the lever trigger
    /// </summary>
    /// <param name="collision">Collision</param>
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var component) && _playerInputActions.Player.Interact.ReadValue<float>() == 1)
        {
            if (!_hasInteracted) Interact(component);
            _hasInteracted = true;
        }
    }
}
