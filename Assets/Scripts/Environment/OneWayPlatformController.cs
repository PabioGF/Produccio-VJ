using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformController : InteractableObject
{
    private BoxCollider2D _boxCollider;
    private bool _enableChange;

    // Start is called before the first frame update
    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _enableChange = true;
    }

    protected override void Interact()
    {
        base.Interact();
        _enableChange = false;
        _boxCollider.enabled = false;
        Invoke(nameof(Reset), 0.2f);
    }

    private void Reset()
    {
        _boxCollider.enabled = true;
        _enableChange = true;
    }
}
