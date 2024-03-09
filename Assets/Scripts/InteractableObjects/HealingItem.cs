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

        if (_playerController.TryGetComponent(out PlayerLifeComponent lifeComponent))
        {
            lifeComponent.Heal(_healingPoints);
            gameObject.SetActive(false);
        }
    }
}
