using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour
{
    public static PlayerInputsManager Instance;

    public InputActionMap inputActionMap;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private UIController _uiController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        //_inputActions.Enable();
        //_inputActions.Player.Jump.performed += JumpInput;
    }

    private void OnDisable()
    {
        //_inputActions.Disable();
        //_inputActions.Player.Jump.performed -= JumpInput;
    }

    
    public float ReadHorizontalInput()
    {
        //return _inputActions.Player.MoveHorizontal.ReadValue<float>();
        return 0f;
    }

    public float ReadVerticalInput()
    {
        //return _inputActions.Player.MoveVertical.ReadValue<float>();
        return 0f;
    }
    
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerController.HandleJumpInput();
        }
    }

    public void FastAttackInput(InputAction.CallbackContext context)
    {

    }
}
