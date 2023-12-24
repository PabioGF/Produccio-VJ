using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformController : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private BoxCollider2D _boxCollider;
    private bool _enableChange;

    // Start is called before the first frame update
    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _enableChange = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_playerInputActions.Player.Interact.ReadValue<float>() == 1 && _playerInputActions.Player.MoveVertical.ReadValue<float>() == -1 && _enableChange)
        {
            _enableChange = false;
            _boxCollider.enabled = false;
            Invoke(nameof(Reset), 0.2f);
        }        
    }

    private void Reset()
    {
        _boxCollider.enabled = true;
        _enableChange = true;
    }
}
