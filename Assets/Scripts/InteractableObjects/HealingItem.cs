using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingScript : InteractableObject
{
    [SerializeField] private int _healingPoints;
    protected override void Interact()
    {
        base.Interact();

        PlayerLifeComponent lifeComponent = _playerController.GetComponentInChildren<PlayerLifeComponent>();

        if (lifeComponent != null)
        {
            lifeComponent.Heal(_healingPoints);
            gameObject.SetActive(false);
        }
        
    }
}