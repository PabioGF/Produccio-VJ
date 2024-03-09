using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformController : InteractableObject
{
    [SerializeField] private BoxCollider2D _boxCollider;

    protected override void Interact()
    {
        if (PlayerInputsManager.Instance.ReadVerticalInput() != -1f) return;

        base.Interact();
        _boxCollider.enabled = false;
        Invoke(nameof(Reset), 0.2f);
    }

    private void Reset()
    {
        _boxCollider.enabled = true;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Doesn't show up the interact button
        if (collision.TryGetComponent(out PlayerController component))
        {
            _playerController = component;
        }
    }
}
