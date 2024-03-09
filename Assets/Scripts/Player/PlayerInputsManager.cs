using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour
{
    public static PlayerInputsManager Instance;

    [Header("Scripts References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerCombat _playerCombat;

    [Header("Input Actions")]
    [SerializeField] private InputAction _horizontalInput;
    [SerializeField] private InputAction _verticalInput;
    [SerializeField] private InputAction _jump;
    [SerializeField] private InputAction _interact;
    [SerializeField] private InputAction _fastAttack;
    [SerializeField] private InputAction _slowAttack;
    [SerializeField] private InputAction _dodgeTrigger;
    [SerializeField] private InputAction _throw;
    [SerializeField] private InputAction _pause;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        // Actions subscribing
        _jump.performed += JumpInput;
        _interact.performed += InteractInput;
        _fastAttack.performed += FastAttackInput;
        _slowAttack.performed += SlowAttackInput;
        _throw.performed += ThrowInput;
        _pause.performed += PauseInput;

    }

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
        _interact.Disable();

        _jump.performed -= JumpInput;
        _interact.performed -= InteractInput;
        _fastAttack.performed -= FastAttackInput;
        _slowAttack.performed -= SlowAttackInput;
        _throw.performed -= ThrowInput;
        _pause.performed -= PauseInput;
    }
    
    public float ReadHorizontalInput()
    {
        return _horizontalInput.ReadValue<float>();
    }

    public float ReadVerticalInput()
    {
        return _verticalInput.ReadValue<float>();
    }

    public float ReadDodgeTriggerValue()
    {
        return _dodgeTrigger.ReadValue<float>();
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerController.HandleJumpInput();
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerController.DesiredInteraction = true;
            UIController.Instance.HideDeathScreen();
        }
    }

    public void FastAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCombat.HandleFastAttackInput();
        }
    }

    public void SlowAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCombat.HandleSlowAttackInput();
        }
    }

    public void ThrowInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCombat.HandleThrowBottleInput();
        }
    }

    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIController.Instance.HandlePauseInput();
        }
    }

    public void EnableControls()
    {
        _horizontalInput.Enable();
        _verticalInput.Enable();
        _jump.Enable();
        _interact.Enable();
        _fastAttack.Enable();
        _slowAttack.Enable();
        _dodgeTrigger.Enable();
        _throw.Enable();
        _pause.Enable();
    }

    public void DisableControls()
    {
        _horizontalInput.Disable();
        _verticalInput.Disable();
        _jump.Disable();
        _fastAttack.Disable();
        _slowAttack.Disable();
        _dodgeTrigger.Disable();
        _throw.Disable();
        _pause.Disable();
    }
}
